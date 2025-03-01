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

        [Header("AnimationClips")] 
        [SerializeField] private ClipTransition _defaultIdleAnimation;
        [SerializeField] private ClipTransition _jumpAnimation;
        [SerializeField] private AnimationPack _walkPack;
        [SerializeField] private AnimationPack _sprintPack;
        [SerializeField] private AnimationPack _crouchPack;

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

        public void UpdateAnimation(CharacterBodyLogger bodyLogger)
        {
            ClipTransition targetAnimation = DetermineTargetAnimation(bodyLogger);

            if (_currentState == null || _currentState.Clip != targetAnimation.Clip)
            {
                _currentState = _animancer.Play(targetAnimation);
            }

            UpdateAnimationParameters(bodyLogger);
        }

        private ClipTransition DetermineTargetAnimation(CharacterBodyLogger bodyLogger)
        {
            if (!bodyLogger.IsGrounded || bodyLogger.CharacterBodyState == ECharacterBodyState.NoClip)
                return _jumpAnimation;

            if (!_animationPacks.TryGetValue(bodyLogger.CharacterBodyState, out var currentPack))
            {
                return _defaultIdleAnimation;
            }

            MovementDirection direction = DetermineMovementDirection(bodyLogger.MoveDirection);
            return currentPack.GetClip(direction);
        }

        private MovementDirection DetermineMovementDirection(Vector2 movementDir)
        {
            if (movementDir.magnitude < 0.1f)
                return MovementDirection.Idle;

            float angle = Vector2.SignedAngle(Vector2.up, movementDir);
            if (angle < -135 || angle >= 135)
                return MovementDirection.MoveBack;
            if (angle >= -135 && angle < -45)
                return MovementDirection.MoveLeft;
            if (angle >= 45 && angle < 135)
                return MovementDirection.MoveRight;
            return MovementDirection.MoveForward;
        }

        private void UpdateAnimationParameters(CharacterBodyLogger bodyLogger)
        {
            // Здесь можно обновить параметры анимации, если нужно
        }
    }
}