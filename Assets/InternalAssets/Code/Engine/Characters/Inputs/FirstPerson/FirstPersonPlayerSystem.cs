using ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Network.Client;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.PlayerInput.FirstPerson
{
    /// <summary>
    /// Система обработки ввода игрока от первого лица.
    /// Собирает пользовательский ввод и передает его соответствующему компоненту FirstPersonInputs.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FirstPersonPlayerSystem : UpdateSystem 
    {
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = World.Filter.With<FirstPersonPlayer>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            uint fixedTick = RuntimeHelper.CurrentFixedStep;
            uint tickrateTick = NetworkTime.LastLocalTick;

            // Сбор данных ввода
            Vector2 moveInput = InputControls.GetMoveAxis();
            Vector2 lookInput = InputControls.GetFirstPersonLookAxis();

            // Предотвращаем движение камеры, если курсор разблокирован
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInput = Vector2.zero;
            }

            // Сбор данных по кнопкам мыши
            bool fireInputPressing = InputControls.GetKey(KeyType.Fire);
            bool fireInputLocked = InputControls.GetKeyDown(KeyType.Fire);
            bool altFireInputPressing = InputControls.GetKey(KeyType.AltFire);
            bool altFireInputLocked = InputControls.GetKeyDown(KeyType.AltFire);
            bool middleInputPressing = InputControls.GetKey(KeyType.Middle);
            bool middleInputLocked = InputControls.GetKeyDown(KeyType.Middle);

            // Сбор данных по действиям
            bool useActionLocked = InputControls.GetKeyDown(KeyType.Use);
            bool sprintAction = InputControls.GetKey(KeyType.Move_Shift);
            bool crouchAction = InputControls.GetKey(KeyType.Move_Crouch);
            bool jumpAction = InputControls.GetKey(KeyType.Move_Jump);
            bool noClipAction = InputControls.GetKeyDown(KeyType.NoClip);

            foreach (var entity in _filter)
            {
                ref var player = ref GetComponent<FirstPersonPlayer>(entity);
                
                if (HasComponent<FirstPersonInputs>(player.ControlledCharacter) && 
                    HasComponent<FirstPersonCharacter>(player.ControlledCharacter))
                {
                    var inputs = GetComponent<FirstPersonInputs>(player.ControlledCharacter.Entity);
                    var character = GetComponent<FirstPersonCharacter>(player.ControlledCharacter.Entity);
                    var characterTransform = GetComponent<Translation>(player.ControlledCharacter.Entity).Transform;

                    // Обработка поворота камеры
                    inputs.LookYawPitchDegrees = lookInput * 2.5f;

                    // Расчет вектора движения относительно ориентации персонажа
                    inputs.MoveVector = (moveInput.y * characterTransform.forward) + 
                                        (moveInput.x * characterTransform.right);
                    inputs.MoveVector = Vector3.ClampMagnitude(inputs.MoveVector, 1f);

                    // Установка состояния кнопок мыши
                    inputs.IsFirePressing = fireInputPressing;
                    inputs.IsAltFirePressing = altFireInputPressing;
                    inputs.IsMiddlePressing = middleInputPressing;

                    // Обработка разовых нажатий для tickrate-системы
                    ProcessButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, 
                        ref inputs.IsFireLocked, fireInputLocked);
                    ProcessButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, 
                        ref inputs.IsAltFireLocked, altFireInputLocked);
                    ProcessButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, 
                        ref inputs.IsMiddleLocked, middleInputLocked);
                    ProcessButtonPress(player.LastInputsProcessingTickrateTick, tickrateTick, 
                        ref inputs.IsUseLocked, useActionLocked);

                    // Обработка разовых нажатий для fixed-системы
                    ProcessButtonPress(player.LastInputsProcessingFixedTick, fixedTick, 
                        ref inputs.JumpRequested, jumpAction);
                    ProcessButtonPress(player.LastInputsProcessingFixedTick, fixedTick, 
                        ref inputs.SprintRequested, sprintAction);
                    ProcessButtonPress(player.LastInputsProcessingFixedTick, fixedTick, 
                        ref inputs.CrouchRequested, crouchAction);
                    ProcessButtonPress(player.LastInputsProcessingFixedTick, fixedTick, 
                        ref inputs.NoClipRequested, noClipAction);
                    
                    // Обновление тиков последней обработки
                    player.LastInputsProcessingFixedTick = fixedTick;
                    player.LastInputsProcessingTickrateTick = tickrateTick;

                    // Применение обновленных входных данных
                    SetComponent(player.ControlledCharacter.Entity, inputs);
                }
            }
        }

        // Обрабатывает разовые нажатия кнопок между тиками
        private void ProcessButtonPress(uint lastTick, uint currentTick, ref bool buttonState, bool inputState)
        {
            if (lastTick == currentTick)
            {
                // Если находимся в том же тике, кнопка считается нажатой, 
                // если она была нажата сейчас или уже была нажата ранее
                buttonState = inputState || buttonState;
            }
            else
            {
                // В новом тике обновляем состояние
                buttonState = inputState;
            }
        }
    }
}