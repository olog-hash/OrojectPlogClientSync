using ProjectOlog.Code.Engine.StateMachines.Interactables;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack
{
    public abstract class KitPackState : InteractionObjectState
    {
        protected KitPackContext Context; 
        
        public KitPackState(KitPackContext context, KitPackStateMachine.EKitPackState key) : base(key)
        {
            Context = context;
        }
    }
}