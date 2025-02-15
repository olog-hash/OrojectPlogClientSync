using ProjectOlog.Code.Game.StateMachines.Interactables;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.KitPack
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