using Animancer;
using ProjectOlog.Code.Game.Animations.Characters.Core;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Game.Animations.Characters.Controllers
{
    public class WeaponAnimationController : MonoBehaviour, IAnimationController
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AvatarMask _avatarMask;
    
        [Header("AnimationClips")] 
        [SerializeField] private ClipTransition _weaponIdleAnimation;
        [SerializeField] private ClipTransition _weaponReloadAnimation;
        [SerializeField] private ClipTransition _weaponShootAnimation;
    
        private AnimancerLayer _weaponLayer;
        private AnimancerState _currentState;

        public void Awake()
        {
            _weaponLayer = _animancer.Layers[2]; // Предполагаем, что слой оружия - третий
            _weaponLayer.SetMask(_avatarMask);
        }

        public void UpdateAnimation(CharacterBodyLogger bodyLogger)
        {
            ClipTransition targetAnimation = DetermineWeaponAnimation(bodyLogger);

            if (_currentState == null || !_currentState.IsPlaying)
            {
                _currentState = _weaponLayer.Play(targetAnimation);
            }
        }

        private ClipTransition DetermineWeaponAnimation(CharacterBodyLogger bodyLogger)
        {

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                return _weaponIdleAnimation;
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
            {
                return _weaponReloadAnimation;
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
            {
                return _weaponShootAnimation;
            }
            
            return _weaponIdleAnimation;
        }
    }
}