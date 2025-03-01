using ProjectOlog.Code.Mechanics.Mortality.Damage;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Replenish.Armor;
using ProjectOlog.Code.Mechanics.Replenish.Health;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
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