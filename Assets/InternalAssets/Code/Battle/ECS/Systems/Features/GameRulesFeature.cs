using ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.DeathCameraTransitionRule;
using ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.SpawnPlayerRequestRule;
using ProjectOlog.Code.Battle.ECS.Rules.SimpleRules;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    
    public sealed class GameRulesFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<ReturnBattleCamereAfterDestroyPlayerSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<SpawnPlayerRequestRuleSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<DeathCameraTransitionRuleSystem>(_systemsGroup);
        }
    }
}