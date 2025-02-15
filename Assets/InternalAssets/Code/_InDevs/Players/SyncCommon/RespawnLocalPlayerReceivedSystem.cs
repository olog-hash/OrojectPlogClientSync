using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Players;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.SyncCommon
{
    /// <summary>
    /// Нужна чтобы уведомить сервер, что клиент получил позицию спавна и уже в ней.
    /// В ином случаи сервер будет считывать какое-то время данные с прошлой позиции сервера, и будет баг с телепортом.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnLocalPlayerReceivedSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        private BasicPlayerNetworker _basicPlayerNetworker;

        public RespawnLocalPlayerReceivedSystem(BasicPlayerNetworker basicPlayerNetworker)
        {
            _basicPlayerNetworker = basicPlayerNetworker;
        }

        public override void OnAwake()
        {
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                SpawnPlayer(spawnEvent);
            }
        }

        public void SpawnPlayer(RespawnPlayerEvent respawnEvent)
        {
            if (respawnEvent.PlayerProvider is null) return;
            var entity = respawnEvent.PlayerProvider.Entity;

            if (!entity.Has<DeadMarker>() && entity.Has<LocalPlayerMarker>())
            {
                _basicPlayerNetworker.RespawnPlayerInfoReceived();
            }
        }
    }
}