using ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson;
using ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching;
using ProjectOlog.Code.Features.Players.Visual.SwitchViewModes.Lifecycle;
using ProjectOlog.Code.Features.Players.Visual.SwitchViewModes.Systems;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
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