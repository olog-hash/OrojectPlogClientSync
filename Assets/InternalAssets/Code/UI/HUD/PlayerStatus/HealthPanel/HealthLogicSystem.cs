using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthLogicSystem : UpdateSystem
    {
        private Filter _localPlayerFilter;
        private HealthViewModel _healthViewModel;

        public HealthLogicSystem(HealthViewModel healthViewModel)
        {
            _healthViewModel = healthViewModel;
        }

        public override void OnAwake()
        {
            _localPlayerFilter = World.Filter.With<LocalPlayerMarker>().With<HealthArmorComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var localPlayerEntity in _localPlayerFilter)
            {
                ref var healthComponent = ref localPlayerEntity.GetComponent<HealthArmorComponent>();
                
                _healthViewModel.MaxHealth = healthComponent.MaxHealth;
                _healthViewModel.CurrentHealth = healthComponent.Health;
                
                _healthViewModel.MaxArmor = 100;
                _healthViewModel.CurrentArmor = healthComponent.Armor;
            }
        }
    }
}