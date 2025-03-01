using System;
using Animancer;

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

        public ClipTransition GetClip(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.Idle => Idle,
                MovementDirection.MoveForward => MoveForward,
                MovementDirection.MoveBack => MoveBack,
                MovementDirection.MoveLeft => MoveLeft,
                MovementDirection.MoveRight => MoveRight,
                _ => Idle
            };
        }
    }
}