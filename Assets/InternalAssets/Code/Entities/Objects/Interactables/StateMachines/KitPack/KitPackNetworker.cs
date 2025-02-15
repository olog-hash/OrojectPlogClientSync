using ProjectOlog.Code.Game.StateMachines.Interactables.Networking;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.KitPack
{
    public class KitPackNetworker : ObjectNetworker
    {
        private KitPackStateMachine _interactionStateMachine;
        private KitPackContext _context;
        
        public KitPackNetworker(KitPackStateMachine interactionStateMachine, KitPackContext context)
        {
            _interactionStateMachine = interactionStateMachine;
            _context = context;
        }
    }
}