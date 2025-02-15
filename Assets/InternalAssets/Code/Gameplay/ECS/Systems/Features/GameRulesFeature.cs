using ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.Rules;
using ProjectOlog.Code.Gameplay.ECS.Rules.GameRules.FirstAppeared.Spawn;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    
    public sealed class GameRulesFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<SpawnPlayerRequestRuleSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<DeathCameraTransitionRuleSystem>(_systemsGroup);
        }
    }
}