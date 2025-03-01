using ProjectOlog.Code.Engine.StateMachines.Interactables.Networking;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.MusicPlayer
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