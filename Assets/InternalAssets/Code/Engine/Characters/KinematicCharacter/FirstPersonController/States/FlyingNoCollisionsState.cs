using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController.States
{
    public class FlyingNoCollisionsState : ICharacterState
    {
        public void OnStateEnter(CharacterState previousState, ref FirstPersonCharacterProcessor processor)
        {
            processor.CharacterBody.SetCollisionDetectionActive(false);
            processor.CharacterBody.ForceUnground();
        }

        public void OnStateExit(CharacterState nextState, ref FirstPersonCharacterProcessor processor)
        {
            processor.CharacterBody.SetCollisionDetectionActive(true);
        }

        public void OnStateUpdate(ref FirstPersonCharacterProcessor processor)
        {
            processor.CharacterCollisionAndGroundingUpdate();

            HandleCharacterControl(ref processor);

            processor.CharacterMovementAndFinalizationUpdate();

            processor.DetectGlobalTransitions();
        }

        public void HandleCharacterControl(ref FirstPersonCharacterProcessor p)
        {
            float verticalInput = 0f + (p.FirstPersonInputs.JumpRequested ? 1f : 0f) + (p.FirstPersonInputs.CrouchRequested ? -1f : 0f);
            Vector3 targetMoveVector = Vector3.ClampMagnitude(p.FirstPersonInputs.MoveVector + (Vector3.up * verticalInput), 1f);
            Vector3 targetVelocity = targetMoveVector * p.FirstPersonCharacter.WalkSpeed;
            CharacterControlUtilities.InterpolateVelocityTowardsTarget(ref p.CharacterBody.BaseVelocity, targetVelocity, p.DeltaTime, p.FirstPersonCharacter.MovementSharpness);
            p.Translation += p.CharacterBody.BaseVelocity * p.DeltaTime;

            p.CharacterBodyLogger.CharacterBodyState = ECharacterBodyState.NoClip;
        }
    }

}
