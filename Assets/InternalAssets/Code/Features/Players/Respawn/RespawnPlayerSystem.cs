using ProjectOlog.Code._InDevs.TranslationUtilits;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Mortality;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Respawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnPlayerSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        
        private EntityMortalityHelper _entityMortalityHelper;
        
        public override void OnAwake()
        {
            _entityMortalityHelper = new EntityMortalityHelper();
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
            if (respawnEvent.PlayerProvider is null || respawnEvent.PlayerProvider.Entity is null) return;
            var playerEntity = respawnEvent.PlayerProvider.Entity;
            
            ref var networkPlayer = ref playerEntity.GetComponent<NetworkPlayer>();

            if (playerEntity.Has<LocalPlayerMarker>())
            {
                CharacterTranslationUtilits.SetPositionAndRotation(
                    playerEntity,
                    respawnEvent.Position,
                    respawnEvent.Rotation);
            }
            else
            {
                RemotePlayerTranslationUtilits.SetPositionAndRotation(
                    playerEntity,
                    respawnEvent.Position,
                    respawnEvent.Rotation);
            }

            // Обновляем последнее состояние
            networkPlayer.LastStateVersion = respawnEvent.LastStateVersion;

            // Возрождаем.
            _entityMortalityHelper.TryReborn(playerEntity);
        }
    }
}