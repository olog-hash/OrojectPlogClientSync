using LiteNetLib.Utils;
using ProjectOlog.Code.Game.StateMachines.Interactables;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor.States
{
    public class OpenedState : SimpleDoorState, IInteractableState
    {
        public OpenedState(SimpleDoorNetworker networker, SimpleDoorContext context, SimpleDoorStateMachine.ESimpleDoorState key) : base(networker, context, key)
        {
            //InteractionStateName = "ЗАКРЫТЬ";
        }

        public override void EnterState()
        {
            Context.InterpolationProvider.GetData().CurrentTransform.rotation = Quaternion.Euler(0, -90, 0);
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
            // Закрываем
            //SetNextState(SimpleDoorStateMachine.ESimpleDoorState.Closing);
        }
    }
}