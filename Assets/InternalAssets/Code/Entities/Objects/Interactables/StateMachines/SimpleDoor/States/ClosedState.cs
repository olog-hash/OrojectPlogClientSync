using LiteNetLib.Utils;
using ProjectOlog.Code.Game.StateMachines.Interactables;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor.States
{
    public class ClosedState : SimpleDoorState, IInteractableState
    {
        public ClosedState(SimpleDoorNetworker networker, SimpleDoorContext context, SimpleDoorStateMachine.ESimpleDoorState key) : base(networker, context, key)
        {
            InteractionStateName = "ОТКРЫТЬ";
        }

        public override void EnterState()
        {
            Context.InterpolationProvider.GetData().CurrentTransform.Rotation = Quaternion.Euler(0, 0, 0);
            Context.InterpolationProvider.GetData().SkipNextInterpolation();
        }

        public override void ExitState()
        {

        }

        public override void UpdateState(float deltaTime)
        {

        }

        public void Interact(EntityProvider entityProvider, NetDataPackage detail)
        {
            // Открываем
            //SetNextState(SimpleDoorStateMachine.ESimpleDoorState.Opening);
        }
    }
}