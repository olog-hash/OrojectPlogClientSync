using ProjectOlog.Code._InDevs.CameraSystem.Player.ViewModes.Lifecycle;
using ProjectOlog.Code._InDevs.CameraSystem.Player.ViewModes.Systems;
using ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.SpectatorPerson;
using ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class SwitchPersonViewFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<SwitchPersonViewInputSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<SpectatorSwitchInputSystem>(_systemsGroup);
            
            // Первостепенное переключение
            _systemsFactory.CreateSystem<InitSwitchPersonViewSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<DeathSwitchPersonViewSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<RespawnSwitchPersonViewSystem>(_systemsGroup);
            
            // Второстепенное переключение
            _systemsFactory.CreateSystem<SpectatorSwitchSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<SpectatorTargetChangeSystem>(_systemsGroup);
            
            // Основное переключение вида.
            _systemsFactory.CreateSystem<SwitchPersonViewSystem>(_systemsGroup);
        }
    }
}