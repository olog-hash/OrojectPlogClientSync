using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using ProjectOlog.Code.UI.HUD.PlayerStats.Presenter;
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
        private PlayerStatsViewModel _playerStatsViewModel;

        public HealthLogicSystem(PlayerStatsViewModel playerStatsViewModel)
        {
            _playerStatsViewModel = playerStatsViewModel;
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
                
                _playerStatsViewModel.HealthArmorModel.SetMaxHealth(healthComponent.MaxHealth);
                _playerStatsViewModel.HealthArmorModel.SetHealth(healthComponent.Health);
                
                _playerStatsViewModel.HealthArmorModel.SetMaxArmor(100);
                _playerStatsViewModel.HealthArmorModel.SetArmor(healthComponent.Armor);
            }
        }
    }
}