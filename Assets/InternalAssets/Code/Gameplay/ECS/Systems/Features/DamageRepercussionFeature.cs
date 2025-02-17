using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Mortality.PostDamage;
using ProjectOlog.Code.Mechanics.Replenish;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class DamageRepercussionFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            BasicSystems(_systemsGroup, _systemsFactory);
            
            _systemsFactory.CreateSystem<PostDamageSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<DeathSystem>(_systemsGroup);
        }
        
        
        private void BasicSystems(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<ReplenishHealthRequestSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<ReplenishArmorRequestSystem>(_systemsGroup);
        }
    }
}