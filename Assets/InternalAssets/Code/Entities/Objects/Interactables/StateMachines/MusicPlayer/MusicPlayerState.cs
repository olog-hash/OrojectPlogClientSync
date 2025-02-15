using ProjectOlog.Code.Game.StateMachines.Interactables;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.MusicPlayer
{
    public abstract class MusicPlayerState : InteractionObjectState
    {
        protected MusicPlayerNetworker Networker;
        protected MusicPlayerContext Context; 
        
        public MusicPlayerState(MusicPlayerNetworker networker, MusicPlayerContext context, MusicPlayerStateMachine.EMusicPlayerState key) : base(key)
        {
            Networker = networker;
            Context = context;
        }
    }
}