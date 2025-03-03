using System.Collections.Generic;
using Animancer;
using ProjectOlog.Code.Engine.Characters.Animations.Core;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.Animations.Controllers
{
    public class LegAnimationController : MonoBehaviour, IAnimationController
    {
        [SerializeField] private AnimancerComponent _animancer;

        [Header("AnimationClips")] [SerializeField]
        private ClipTransition _defaultIdleAnimation;

        [SerializeField] private ClipTransition _jumpAnimation;
        [SerializeField] private AnimationPack _walkPack;
        [SerializeField] private AnimationPack _sprintPack;
        [SerializeField] private AnimationPack _crouchPack;

        [Header("Animation Speeds")]
        [Range(0.5f, 2.0f)]
        [SerializeField] private float _idleSpeed = 1.0f;
        [Range(0.5f, 2.0f)]
        [SerializeField] private float _jumpSpeed = 1.0f;
        
        private Dictionary<ECharacterBodyState, AnimationPack> _animationPacks;
        private AnimancerState _currentState;

        public void Awake()
        {
            InitializeAnimationPacks();
        }

        private void InitializeAnimationPacks()
        {
            _animationPacks = new Dictionary<ECharacterBodyState, AnimationPack>
            {
                { ECharacterBodyState.Walk, _walkPack },
                { ECharacterBodyState.Sprint, _sprintPack },
                { ECharacterBodyState.Crouch, _crouchPack }
            };

            // Убедимся, что у каждого пака есть анимация Idle, если нет - используем дефолтную
            foreach (var pack in _animationPacks.Values)
            {
                if (pack.Idle == null)
                {
                    pack.Idle = _defaultIdleAnimation;
                }
            }
        }

        // Основной метод обновления анимации - использует только данные из CharacterBodyLogger
        public void UpdateAnimation(CharacterBodyLogger bodyLogger)
        {
            // Определяем целевую анимацию по данным из bodyLogger
            (ClipTransition targetAnimation, float speed) = DetermineTargetAnimation(bodyLogger);

            // Воспроизводим анимацию, если она изменилась или нужно обновить скорость
            if (_currentState == null || _currentState.Clip != targetAnimation.Clip)
            {
                _currentState = _animancer.Play(targetAnimation);
                _currentState.Speed = speed; // Устанавливаем скорость анимации
            }
            else if (_currentState.Speed != speed)
            {
                _currentState.Speed = speed; // Обновляем только скорость, если анимация не изменилась
            }
        }

        private (ClipTransition, float) DetermineTargetAnimation(CharacterBodyLogger bodyLogger)
        {
            // Если персонаж в воздухе или в режиме прыжка
            if (!bodyLogger.IsGrounded || bodyLogger.CharacterBodyState == ECharacterBodyState.Jump || 
                bodyLogger.CharacterBodyState == ECharacterBodyState.NoClip)
            {
                return (_jumpAnimation, _jumpSpeed);
            }

            // Если персонаж в покое и не двигается
            if (bodyLogger.CharacterBodyState == ECharacterBodyState.Idle || 
                bodyLogger.CharacterBodyState == ECharacterBodyState.None)
            {
                return (_defaultIdleAnimation, _idleSpeed);
            }

            // Получаем нужный пакет анимаций для текущего состояния тела
            if (_animationPacks.TryGetValue(bodyLogger.CharacterBodyState, out var animationPack))
            {
                // Получаем анимацию из пакета на основе направления
                var clip = animationPack.GetClip(bodyLogger.MovementDirection);
                
                // Для Idle используем более медленную скорость, даже если это из пакета
                float speed = bodyLogger.MovementDirection == DetailedMovementDirection.Idle 
                    ? _idleSpeed 
                    : animationPack.AnimationSpeed;
                    
                return (clip, speed);
            }

            // Если не нашли подходящий пакет анимаций, используем анимацию покоя
            return (_defaultIdleAnimation, _idleSpeed);
        }


        private void UpdateAnimationParameters(CharacterBodyLogger bodyLogger)
        {
            // Здесь можно обновить параметры анимации, если нужно
        }
    }
}