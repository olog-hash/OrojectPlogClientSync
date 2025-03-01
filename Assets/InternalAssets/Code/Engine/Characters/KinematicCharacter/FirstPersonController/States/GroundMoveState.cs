using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController.States
{
    public class GroundMoveState : ICharacterState
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
            DefineCharacterState(ref p, out float groundMaxSpeed);

            if (p.CharacterBody.GroundingStatus.IsStableOnGround)
            {
                // Move on ground
                Vector3 targetVelocity = p.FirstPersonInputs.MoveVector * groundMaxSpeed;
                CharacterControlUtilities.StandardGroundMove_Interpolated(ref p.CharacterBody.BaseVelocity, targetVelocity, p.FirstPersonCharacter.MovementSharpness, p.DeltaTime, p.FirstPersonCharacter.GroundingUp, p.CharacterBody.GroundingStatus.GroundNormal);

                // Jump
                if (p.FirstPersonInputs.JumpRequested && !p.FirstPersonInputs.CrouchRequested)
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
            
            CharacterControlUtilities.InternalVelocityApply(ref p.CharacterBody, ref p.FirstPersonCharacter.InternalVelocityAdd);
            CharacterControlUtilities.CameraPositionUpdate(ref p.FirstPersonCharacter.CameraPointHeight, p.FirstPersonInputs.CrouchRequested, p.DeltaTime);
        }

        private void DefineCharacterState(ref FirstPersonCharacterProcessor p, out float groundMaxSpeed)
        {
            if (p.FirstPersonInputs.CrouchRequested)
            {
                groundMaxSpeed = p.FirstPersonCharacter.CrouchSpeed;
                SetCharacterBodyState(ref p, ECharacterBodyState.Crouch);
            }
            else
            {
                if (p.FirstPersonInputs.SprintRequested)
                {
                    groundMaxSpeed = p.FirstPersonCharacter.SprintSpeed;
                    SetCharacterBodyState(ref p, ECharacterBodyState.Sprint);
                }
                else
                {
                    groundMaxSpeed = p.FirstPersonCharacter.WalkSpeed;
                    SetCharacterBodyState(ref p, ECharacterBodyState.Walk);
                }
            }
        }

        private void SetCharacterBodyState(ref FirstPersonCharacterProcessor p, ECharacterBodyState characterBodyState)
        {
            p.CharacterBodyLogger.CharacterBodyState = characterBodyState;
        }
        
        public bool DetectTransitions(ref FirstPersonCharacterProcessor p)
        {
            /*
            if (p.FirstPersonInputs.CrouchRequested)
            {
                //p.TransitionToState(CharacterState.Crouched);
                //return true;
            }

            if (!p.CharacterBody.GroundingStatus.IsStableOnGround)
            {
                p.TransitionToState(CharacterState.AirMove);
                return true;
            }
            */
            return false;
        }
    }


}
