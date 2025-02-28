using ProjectOlog.Code.Game.Animations.Characters;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    
    public sealed class PlayerAnimationsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<CharacterAnimationLegsSystem>(_systemsGroup);
        }
    }
}