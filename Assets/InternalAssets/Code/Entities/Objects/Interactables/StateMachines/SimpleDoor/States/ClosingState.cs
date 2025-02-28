using ProjectOlog.Code.Entities.Objects.Interactables.Core;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor.States
{
    public class ClosingState : SimpleDoorState
    {
        private RotateInterpolation _rotateInterpolation;
        
        public ClosingState(SimpleDoorNetworker networker, SimpleDoorContext context, SimpleDoorStateMachine.ESimpleDoorState key) : base(networker, context, key)
        {
            _rotateInterpolation = new RotateInterpolation(Context.PivotTransform, Vector3.zero, 0.2f);
        }

        public override void EnterState()
        {
            _rotateInterpolation.StartRotation();
            Context.AudioSource.PlayOneShot(Context.CloseDoorClip);
        }

        public override void ExitState()
        {

        }

        public override void UpdateState(float deltaTime)
        {
            _rotateInterpolation.OnUpdate(deltaTime);
            
            Context.InterpolationProvider.GetData().CurrentTransform.Rotation = _rotateInterpolation.CurrentRotation;
        }
    }
}