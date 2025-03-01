using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController.States
{
    public class AirMoveState : ICharacterState
    {
        public void OnStateEnter(CharacterState previousState, ref FirstPersonCharacterProcessor processor)
        {

        }

        public void OnStateExit(CharacterState nextState, ref FirstPersonCharacterProcessor processor)
        {

        }

        public void OnStateUpdate(ref FirstPersonCharacterProcessor processor)
        {
            processor.CharacterCollisionAndGroundingUpdate();

            HandleCharacterControl(ref processor);

            processor.CharacterMovementAndFinalizationUpdate();

            if (!DetectTransitions(ref processor))
            {
                processor.DetectGlobalTransitions();
            }
        }

        public void HandleCharacterControl(ref FirstPersonCharacterProcessor p)
        {
            float GroundMaxSpeed = p.FirstPersonCharacter.WalkSpeed;

            if (p.CharacterBody.GroundingStatus.IsStableOnGround)
            {
                // Move on ground
                Vector3 targetVelocity = p.FirstPersonInputs.MoveVector * GroundMaxSpeed;
                CharacterControlUtilities.StandardGroundMove_Interpolated(ref p.CharacterBody.BaseVelocity, targetVelocity, p.FirstPersonCharacter.MovementSharpness, p.DeltaTime, p.FirstPersonCharacter.GroundingUp, p.CharacterBody.GroundingStatus.GroundNormal);

                // Jump
                if (p.FirstPersonInputs.JumpRequested)
                {
                    CharacterControlUtilities.StandardJump(ref p.CharacterBody, p.FirstPersonCharacter.GroundingUp * p.FirstPersonCharacter.JumpSpeed, true, p.FirstPersonCharacter.GroundingUp);
                }
            }
            else
            {
                Vector3 airAcceleration = p.FirstPersonInputs.MoveVector * p.FirstPersonCharacter.AirAcceleration;
                CharacterControlUtilities.StandardAirMove(ref p.CharacterBody.BaseVelocity, airAcceleration, p.FirstPersonCharacter.AirMaxSpeed, p.FirstPersonCharacter.GroundingUp, p.DeltaTime, false);

                // Gravity
                CharacterControlUtilities.AccelerateVelocity(ref p.CharacterBody.BaseVelocity, p.FirstPersonCharacter.Gravity, p.DeltaTime);

                // Drag
                CharacterControlUtilities.ApplyDragToVelocity(ref p.CharacterBody.BaseVelocity, p.DeltaTime, p.FirstPersonCharacter.AirDrag);
            }
        }

        public bool DetectTransitions(ref FirstPersonCharacterProcessor p)
        {
            if (p.CharacterBody.GroundingStatus.IsStableOnGround)
            {
                p.TransitionToState(CharacterState.GroundMove);
                return true;
            }

            return false;
        }
    }

}
