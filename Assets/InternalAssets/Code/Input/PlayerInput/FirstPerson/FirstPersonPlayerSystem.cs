using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Input.Controls;
using ProjectOlog.Code.Networking.Client;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Input.PlayerInput.FirstPerson
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FirstPersonPlayerSystem : UpdateSystem 
    {
        private Filter filter;

        public override void OnAwake()
        {
            filter = World.Filter.With<FirstPersonPlayer>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            uint fixedTick = RuntimeHelper.CurrentFixedStep;
            uint tickrateTick = NetworkTime.LastLocalTick;

            // Gather input
            Vector2 moveInput = InputControls.GetMoveAxis();
            Vector2 lookInput = InputControls.GetFirstPersonLookAxis();

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInput = Vector2.zero;
            }

            bool fireInputPressing = InputControls.GetKey(KeyType.Fire);
            bool fireInputLocked = InputControls.GetKeyDown(KeyType.Fire);

            bool altFireInputPressing = InputControls.GetKey(KeyType.AltFire);
            bool altFireInputLocked = InputControls.GetKeyDown(KeyType.AltFire);

            bool middleInputPressing = InputControls.GetKey(KeyType.Middle);
            bool middleInputLocked = InputControls.GetKeyDown(KeyType.Middle);

            bool useLocked = InputControls.GetKeyDown(KeyType.Use);

            bool sprintInput = InputControls.GetKey(KeyType.Move_Shift);
            bool crouchInput = InputControls.GetKey(KeyType.Move_Crouch);
            bool jumpInput = InputControls.GetKey(KeyType.Move_Jump);
            bool noClipInput = InputControls.GetKeyDown(KeyType.NoClip);

            
            foreach (var entity in filter)
            {
                ref var player = ref GetComponent<FirstPersonPlayer>(entity);
                
                if (HasComponent<FirstPersonInputs>(player.ControlledCharacter) && HasComponent<FirstPersonCharacter>(player.ControlledCharacter))
                {
                    var inputs = GetComponent<FirstPersonInputs>(player.ControlledCharacter.Entity);
                    var character = GetComponent<FirstPersonCharacter>(player.ControlledCharacter.Entity);
                    var characterTransform = GetComponent<Translation>(player.ControlledCharacter.Entity).Transform;

                    // Look
                    inputs.LookYawPitchDegrees = lookInput * 2.5f;

                    // Move
                    inputs.MoveVector = (moveInput.y * characterTransform.forward) + (moveInput.x * characterTransform.right);
                    inputs.MoveVector = Vector3.ClampMagnitude(inputs.MoveVector, 1f);

                    // Mouse
                    inputs.IsFirePressing = fireInputPressing;
                    inputs.IsAltFirePressing = altFireInputPressing;
                    inputs.IsMiddlePressing = middleInputPressing;

                    // Punctual input presses need special handling when they will be used in a fixed step system.
                    // We essentially need to remember if the button was pressed at any point over the last fixed update
                    ButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, ref inputs.IsFireLocked, fireInputLocked);
                    ButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, ref inputs.IsAltFireLocked, altFireInputLocked);
                    ButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, ref inputs.IsMiddleLocked, middleInputLocked);

                    ButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, ref inputs.IsUseLocked, useLocked);

                    ButtonPress(player.LastInputsProcessingFixedTick, fixedTick, ref inputs.JumpRequested, jumpInput);
                    ButtonPress(player.LastInputsProcessingFixedTick, fixedTick, ref inputs.SprintRequested, sprintInput);
                    ButtonPress(player.LastInputsProcessingFixedTick, fixedTick, ref inputs.CrouchRequested, crouchInput);
                    ButtonPress(player.LastInputsProcessingFixedTick, fixedTick, ref inputs.NoClipRequested, noClipInput);
                    
                    // Если умер
                    //if (player.ControlledCharacter.Entity.Has<DeadMarker>())
                    //{
                    //    inputs = new FirstPersonInputs();
                    //}
                    
                    player.LastInputsProcessingFixedTick = fixedTick;
                    player.LastInputsProcessingTickrateTick = tickrateTick;

                    SetComponent(player.ControlledCharacter.Entity, inputs);
                }
            }
        }

        private void ButtonPress(uint LastTick, uint FixedTick, ref bool ButtonPressed, bool Input)
        {
            if (LastTick == FixedTick)
            {
                ButtonPressed = Input || ButtonPressed;
            }
            else
            {
                ButtonPressed = Input;
            }
        }
    }
}