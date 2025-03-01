namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack.States
{
    public sealed class DefaultState : KitPackState
    {
        public DefaultState(KitPackContext context, KitPackStateMachine.EKitPackState key) : base(context, key)
        {
            
        }

        public override void EnterState()
        {
            Context.KitPackModel.SetActive(true);
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState(float deltaTime)
        {
            
        }
    }
}