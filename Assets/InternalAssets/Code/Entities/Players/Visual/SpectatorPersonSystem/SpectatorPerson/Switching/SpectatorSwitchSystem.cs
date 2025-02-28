using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpectatorSwitchSystem : TickrateSystem
    {
        private Filter _spectatorSwitchingFilter;
        private SpectatorTargetSelector _spectatorTargetSelector;
        
        public override void OnAwake()
        {
            _spectatorSwitchingFilter = World.Filter.With<SpectatorSwitchRequestEvent>().Build();
            _spectatorTargetSelector = new SpectatorTargetSelector();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _spectatorSwitchingFilter)
            {
                ref var spectatorSwitchingEvent = ref entityEvent.GetComponent<SpectatorSwitchRequestEvent>();
                var nextTarget = _spectatorTargetSelector.SelectNextTarget();
                
                // Создаем запрос на переключение к следующей цели
                World.CreateTickEvent().AddComponentData(new SpectatorTargetChangeRequestEvent()
                {
                    SpectatorTarget = nextTarget
                });
            }
        }
    }
}