using ProjectOlog.Code.Engine.Characters.Animations;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    
    public sealed class PlayerAnimationsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<CharacterAnimationLegsSystem>(_systemsGroup);
        }
    }
}