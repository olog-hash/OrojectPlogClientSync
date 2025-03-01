using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Features.Players.Respawn;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Visual.SwitchViewModes.Lifecycle
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnSwitchPersonViewSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        
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
            
            if (!entity.Has<LocalPlayerMarker>()) return;
            
            World.CreateTickEvent().AddComponentData(new SwitchPersonViewEvent
            {
                ViewType = EPersonViewType.First
            });
        }
    }
}