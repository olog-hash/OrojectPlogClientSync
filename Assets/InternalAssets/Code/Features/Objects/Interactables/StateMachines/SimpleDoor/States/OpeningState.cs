using ProjectOlog.Code.Features.Objects.Interactables.Core;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.SimpleDoor.States
{
    public class OpeningState : SimpleDoorState
    {
        private RotateInterpolation _rotateInterpolation;
        
        public OpeningState(SimpleDoorNetworker networker, SimpleDoorContext context, SimpleDoorStateMachine.ESimpleDoorState key) : base(networker, context, key)
        {
            _rotateInterpolation = new RotateInterpolation(Context.PivotTransform, new Vector3(0, -90f, 0), 0.2f);
        }

        public override void EnterState()
        {
            _rotateInterpolation.StartRotation();
            Context.AudioSource.PlayOneShot(Context.OpenDoorClip);
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