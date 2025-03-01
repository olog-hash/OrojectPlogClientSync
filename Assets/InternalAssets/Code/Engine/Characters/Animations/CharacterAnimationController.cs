using System.Collections.Generic;
using Animancer;
using ProjectOlog.Code.Engine.Characters.Animations.Controllers;
using ProjectOlog.Code.Engine.Characters.Animations.Core;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.Animations
{
    [DefaultExecutionOrder(-10000)]
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private LegAnimationController _legAnimationController;
        [SerializeField] private BackAnimationController _backAnimationController;
        
        [SerializeField] private CharacterBodyLogger _bodyLogger;
    
        private List<IAnimationController> _animationControllers;
        

        private void Awake()
        {
            _animationControllers = new List<IAnimationController>
            {
                _legAnimationController,
                _backAnimationController,
                //new BackAnimationController(_animancer),
                //new WeaponAnimationController(_animancer)
            };
        }

        private void Update()
        {
            foreach (var controller in _animationControllers)
            {
                controller.UpdateAnimation(_bodyLogger);
            }
        }
    }
}