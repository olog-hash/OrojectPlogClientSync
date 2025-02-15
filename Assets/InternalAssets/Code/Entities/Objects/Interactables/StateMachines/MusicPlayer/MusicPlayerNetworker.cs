using ProjectOlog.Code.Game.StateMachines.Interactables.Networking;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.MusicPlayer
{
    public class MusicPlayerNetworker : ObjectNetworker
    {
        private MusicPlayerStateMachine _interactionStateMachine;
        private MusicPlayerContext _context;
        
        public MusicPlayerNetworker(MusicPlayerStateMachine interactionStateMachine, MusicPlayerContext context)
        {
            _interactionStateMachine = interactionStateMachine;
            _context = context;
        }
    }
}