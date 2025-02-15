using Animancer;
using ProjectOlog.Code.Game.Animations.Characters.Core;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Game.Animations.Characters.Controllers
{
    public class BackAnimationController : MonoBehaviour, IAnimationController
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AvatarMask _avatarMask;
        
        [Header("AnimationClips")] 
        [SerializeField] private ClipTransition _lookUpDownAnimation;
    
        private AnimancerLayer _backLayer;
        private AnimancerState _currentState;

        public void Awake()
        {
            _backLayer = _animancer.Layers[1]; // Предполагаем, что слой спины - второй
            _backLayer.SetMask(_avatarMask);
        }

        public void UpdateAnimation(CharacterBodyLogger bodyLogger)
        {
            if (_currentState == null || _currentState.Clip != _lookUpDownAnimation.Clip)
            {
                _currentState = _backLayer.Play(_lookUpDownAnimation);
            }

            // Нормализуем угол наклона от -90 до 90 градусов в диапазон от 0 до 1
            float normalizedPitch = Mathf.InverseLerp(-90f, 90f, -bodyLogger.ViewPitchDegrees);
        
            // Устанавливаем время анимации в соответствии с углом наклона
            _currentState.Time = normalizedPitch * _currentState.Duration;
            _currentState.IsPlaying = false;
        }
    }
}