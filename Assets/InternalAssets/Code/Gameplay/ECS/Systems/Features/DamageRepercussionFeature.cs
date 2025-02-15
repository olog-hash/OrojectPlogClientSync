using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.DamageRequest;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Entities.Player.Death;
using ProjectOlog.Code.Mechanics.Replenish;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class DamageRepercussionFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<PostDamageSystem>(_systemsGroup);

            BasicSystems(_systemsGroup, _systemsFactory);
            PlayerSystems(_systemsGroup, _systemsFactory);
        }

        private void PlayerSystems(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<PlayerDeathSystem>(_systemsGroup);
        }
        
        private void BasicSystems(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<ReplenishHealthRequestSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<ReplenishArmorRequestSystem>(_systemsGroup);
        }
    }
}