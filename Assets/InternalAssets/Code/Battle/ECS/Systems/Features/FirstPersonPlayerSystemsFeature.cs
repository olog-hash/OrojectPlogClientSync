using ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Engine.Characters.PlayerInput.FirstPerson;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class FirstPersonPlayerSystemsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<FirstPersonPlayerSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<FirstPersonCharacterMovementSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<FirstPersonCharacterRotationSystem>(_systemsGroup);
        }
    }
}