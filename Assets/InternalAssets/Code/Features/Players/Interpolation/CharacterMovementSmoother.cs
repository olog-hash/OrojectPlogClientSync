using System;
using ProjectOlog.Code.Engine.Characters.Animations.Core;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Interpolation
{
    /// <summary>
    /// Класс для сглаживания движений удаленных игроков для предотвращения дрожания анимации
    /// </summary>
     public class CharacterMovementSmoother
    {
        private Vector3 _previousPosition;
        private Quaternion _previousRotation;
        private Vector2 _smoothedMoveVector = Vector2.zero;
        
        // Коэффициенты сглаживания
        private float _deltaSmooth = 0.2f;
        private float _deltaRotationSmooth = 0.2f;
        
        // Пороги для определения движения
        private const float IDLE_THRESHOLD = 0.0001f;
        private const float ROTATION_IDLE_THRESHOLD = 4.0f;
        
        private float _lastUpdateTime;

        public void UpdateMovement(Vector3 position, Quaternion rotation)
        {
            float deltaTime = Time.deltaTime;
            
            if (_previousPosition == Vector3.zero)
            {
                // Инициализация при первом вызове
                _previousPosition = position;
                _previousRotation = rotation;
                _lastUpdateTime = Time.time;
                return;
            }
            
            // Проверяем изменение позиции
            Vector3 positionDelta = position - _previousPosition;
            if (positionDelta.sqrMagnitude < IDLE_THRESHOLD)
            {
                // Если персонаж почти не двигается, плавно уменьшаем вектор движения
                _smoothedMoveVector = Vector2.Lerp(_smoothedMoveVector, Vector2.zero, deltaTime * 5f);
                
                // Проверяем вращение на месте
                float rotationDelta = Quaternion.Angle(_previousRotation, rotation);
                if (rotationDelta > ROTATION_IDLE_THRESHOLD)
                {
                    // Если персонаж поворачивается на месте, создаем имитацию движения в сторону
                    float rotationDirection = Mathf.Sign(rotation.eulerAngles.y - _previousRotation.eulerAngles.y);
                    if (Mathf.Abs(rotation.eulerAngles.y - _previousRotation.eulerAngles.y) > 180f)
                        rotationDirection = -rotationDirection;
                    
                    _smoothedMoveVector = new Vector2(rotationDirection, 0) * (rotationDelta / 90f);
                }
            }
            else
            {
                // Вычисляем вектор движения в локальном пространстве персонажа
                Vector2 currentMoveVector = CalculateMoveVector(_previousPosition, position, rotation);
                
                // Сглаживаем движение для плавности анимаций
                _smoothedMoveVector = Vector2.Lerp(_smoothedMoveVector, currentMoveVector, _deltaSmooth);
                
                _lastUpdateTime = Time.time;
            }
            
            // Обновляем предыдущие значения для следующего кадра
            _previousPosition = position;
            _previousRotation = rotation;
        }
        
        public Vector2 GetSmoothedMoveVector()
        {
            // Если давно не было обновлений, постепенно уменьшаем движение
            if (Time.time - _lastUpdateTime > 0.2f)
            {
                _smoothedMoveVector = Vector2.Lerp(_smoothedMoveVector, Vector2.zero, Time.deltaTime * 5f);
            }
            
            return _smoothedMoveVector;
        }
        
        private Vector2 CalculateMoveVector(Vector3 previousPosition, Vector3 currentPosition, Quaternion currentRotation)
        {
            // Вычисляем дельту позиции
            float deltaX = currentPosition.x - previousPosition.x;
            float deltaZ = currentPosition.z - previousPosition.z;
            
            Vector2 inputMoveDirection = new Vector2(deltaX, deltaZ);
            
            // Преобразуем в локальное пространство персонажа
            float rotationAngle = currentRotation.eulerAngles.y * Mathf.Deg2Rad;
            
            float sin = Mathf.Sin(rotationAngle);
            float cos = Mathf.Cos(rotationAngle);
            
            // Поворачиваем вектор движения относительно поворота персонажа
            Vector2 rotatedMoveDirection = new Vector2(
                inputMoveDirection.x * cos - inputMoveDirection.y * sin,
                inputMoveDirection.x * sin + inputMoveDirection.y * cos
            );
            
            // Инвертируем X для правильного направления
            rotatedMoveDirection.x = -rotatedMoveDirection.x;
            
            return rotatedMoveDirection.normalized * inputMoveDirection.magnitude * 10f; // Множитель для усиления сигнала
        }
    }
}