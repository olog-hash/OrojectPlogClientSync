using ProjectOlog.Code.Game.StateMachines.Interactables.Networking;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.SimpleDoor
{
    public class SimpleDoorNetworker : ObjectNetworker
    {
        private SimpleDoorStateMachine _interactionStateMachine;
        private SimpleDoorContext _context;
        
        public SimpleDoorNetworker(SimpleDoorStateMachine interactionStateMachine, SimpleDoorContext context)
        {
            _interactionStateMachine = interactionStateMachine;
            _context = context;
        }
        
        
    }
}