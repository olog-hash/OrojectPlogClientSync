using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
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