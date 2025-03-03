using System;
using Animancer;
using ProjectOlog.Code.Engine.Characters.Animations.Controllers;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.Animations.Core
{
    [Serializable]
    public class AnimationPack
    {
        public ClipTransition Idle;
        public ClipTransition MoveForward;
        public ClipTransition MoveBack;
        public ClipTransition MoveLeft;
        public ClipTransition MoveRight;

        [Header("Animation Speeds")]
        [Range(0.5f, 2.0f)]
        public float AnimationSpeed = 1.0f;
        
        public ClipTransition GetClip(DetailedMovementDirection direction)
        {
            return direction switch
            {
                DetailedMovementDirection.Idle => Idle,
                DetailedMovementDirection.Forward => MoveForward,
                DetailedMovementDirection.Backward => MoveBack,
                DetailedMovementDirection.StrafeLeft => MoveLeft,
                DetailedMovementDirection.StrafeRight => MoveRight,
                _ => Idle
            };
        }
    }
}