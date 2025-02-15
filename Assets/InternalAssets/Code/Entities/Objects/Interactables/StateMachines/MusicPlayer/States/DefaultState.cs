using LiteNetLib.Utils;
using ProjectOlog.Code.Game.StateMachines.Interactables;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.MusicPlayer.States
{
    public sealed class DefaultState : MusicPlayerState, IInteractableState
    {
        public DefaultState(MusicPlayerNetworker networker, MusicPlayerContext context, MusicPlayerStateMachine.EMusicPlayerState key) : base(networker, context, key)
        {
            InteractionStateName = "ВКЛЮЧИТЬ";
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState(float deltaTime)
        {
            
        }

        public void Interact(EntityProvider entityProvider, NetDataPackage detail)
        {
            //SetNextState(MusicPlayerStateMachine.EMusicPlayerState.Playing);
        }
    }
}