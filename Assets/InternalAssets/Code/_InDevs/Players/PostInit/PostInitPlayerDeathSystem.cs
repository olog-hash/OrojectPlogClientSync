using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.PostInit
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PostInitPlayerDeathSystem : TickrateSystem
    {
        private Filter _postInitPlayerEventFilter;
        
        public override void OnAwake()
        {
            _postInitPlayerEventFilter = World.Filter.With<PostInitPlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _postInitPlayerEventFilter)
            {
                ref var postInitPlayerEvent = ref entityEvent.GetComponent<PostInitPlayerEvent>();
                
                PostInitPlayerEvent(ref postInitPlayerEvent);
            }
        }

        private void PostInitPlayerEvent(ref PostInitPlayerEvent postInitPlayerEvent)
        {
            if (postInitPlayerEvent.PlayerProvider.Has<LocalPlayerMarker>()) return;
            
            // Если игрок мертвый - создаем ивент о его смерти
            if (postInitPlayerEvent.IsDead)
            {
                CreateFakeDeathEvent(postInitPlayerEvent.PlayerProvider.Entity);
            }
        }

        private void CreateFakeDeathEvent(Entity playerEntity)
        {
            var tickEvent = World.CreateTickEvent().AddVirtualMarker();
            
            tickEvent.AddComponentData(new DeathEvent()
            {
                VictimEntity = playerEntity
            });

            tickEvent.AddComponent<PlayerVictimMarker>();
        }
    }
}