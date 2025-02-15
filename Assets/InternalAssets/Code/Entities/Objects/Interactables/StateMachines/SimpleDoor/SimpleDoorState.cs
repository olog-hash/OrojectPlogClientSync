using ProjectOlog.Code.Game.StateMachines.Interactables;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor
{
    public abstract class SimpleDoorState : InteractionObjectState
    {
        protected SimpleDoorNetworker Networker;
        protected SimpleDoorContext Context; 
        
        public SimpleDoorState(SimpleDoorNetworker networker, SimpleDoorContext context, SimpleDoorStateMachine.ESimpleDoorState key) : base(key)
        {
            Networker = networker;
            Context = context;
        }
    }
}