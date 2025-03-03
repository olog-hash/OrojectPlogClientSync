using System;
using ProjectOlog.Code.Engine.Characters.Animations.Controllers;
using ProjectOlog.Code.Engine.Characters.Animations.Core;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Interpolation
{
    /// <summary>
    /// Класс для сглаживания движений удаленных игроков и определения направления движения
    /// </summary>
    public class CharacterMovementSmoother
    {
        // Константы для определения порогов движения (из ActorAnimator)
        private const float IDLE_THRESHOLD = 0.0001f;
        private const float STRAFE_THRESHOLD = 0.1f;
        private const float ROTATION_IDLE_THRESHOLD = 4f;
        private const int IDLE_PERIOD = 1000000; // ~0.1 секунды
        
        private const float VELOCITY_IDLE_THRESHOLD = 0.05f;
        
        // Параметры сглаживания
        private float deltaSmooth = 0.2f;
        private float deltaRotationSmooth = 0.2f;
        
        // Текущие значения дельт движения
        private float deltaX;
        private float deltaZ;
        private float deltaR; // вращение
        
        // Хранение предыдущего состояния
        private Vector3 lastPosition = Vector3.zero;
        private Vector3 lastRotation = Vector3.zero;
        private long lastPositionTime = DateTime.Now.Ticks;

        /// <summary>
        /// Обновляет состояние движения на основе текущего положения и вращения
        /// и записывает направление в bodyLogger
        /// </summary>
        public void UpdateMovement(Vector3 position, Quaternion rotation, ref CharacterBodyLogger bodyLogger)
        {
            // Если это первый вызов, инициализируем
            if (lastPosition == Vector3.zero)
            {
                lastPosition = position;
                lastRotation = rotation.eulerAngles;
                bodyLogger.MovementDirection = DetailedMovementDirection.Idle;
                return;
            }
            
            float deltaTime = (float)(DateTime.Now.Ticks - lastPositionTime) / 1E+07f;
            if (deltaTime < 0.0001f) deltaTime = 0.016f; // Защита от деления на ноль
            
            float rotationDelta = Mathf.DeltaAngle(lastRotation.y, rotation.eulerAngles.y) / deltaTime;
            Vector3 direction = position - lastPosition;
            
            // Проверка на неподвижность (включая вращение на месте)
            if (direction.sqrMagnitude < IDLE_THRESHOLD)
            {
                // При вращении на месте тоже устанавливаем Idle
                bodyLogger.MovementDirection = DetailedMovementDirection.Idle;
                
                if (DateTime.Now.Ticks - lastPositionTime > IDLE_PERIOD)
                {
                    // Обновляем время и состояние для длительного бездействия
                    lastPositionTime = DateTime.Now.Ticks;
                }
                
                // Обновляем вращение для последующих вычислений
                lastRotation = rotation.eulerAngles;
                
                // Сбрасываем дельты движения
                deltaX = 0f;
                deltaZ = 0f;
                deltaR = 0f;
                return;
            }
            
            // Обновление времени последнего движения
            lastPositionTime = DateTime.Now.Ticks;
            lastRotation = rotation.eulerAngles;
            
            // Обновление последней позиции
            lastPosition = position;
            
            // Преобразование в локальное пространство (как в ActorAnimator)
            Vector3 localDir = Quaternion.Inverse(rotation) * direction;
            
            // Новый расчет скорости движения
            float targetX = localDir.x / deltaTime;
            float targetZ = localDir.z / deltaTime;
            
            // Сглаживание движения
            deltaX = Mathf.Lerp(deltaX, targetX, deltaSmooth);
            deltaZ = Mathf.Lerp(deltaZ, targetZ, deltaSmooth);
            
            if (Mathf.Abs(deltaX) < VELOCITY_IDLE_THRESHOLD && Mathf.Abs(deltaZ) < VELOCITY_IDLE_THRESHOLD)
            {
                bodyLogger.MovementDirection = DetailedMovementDirection.Idle;
                return;
            }
            
            // Определение направления движения на основе дельт и пороговых значений
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaZ) + STRAFE_THRESHOLD)
            {
                // Преобладает боковое движение
                if (deltaX > 0f)
                {
                    bodyLogger.MovementDirection = DetailedMovementDirection.StrafeRight;
                }
                else
                {
                    bodyLogger.MovementDirection = DetailedMovementDirection.StrafeLeft;
                }
            }
            else
            {
                // Преобладает движение вперед/назад
                if (deltaZ > 0f)
                {
                    bodyLogger.MovementDirection = DetailedMovementDirection.Forward;
                }
                else
                {
                    bodyLogger.MovementDirection = DetailedMovementDirection.Backward;
                }
            }
        }
    }
}