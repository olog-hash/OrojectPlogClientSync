namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.KitPack.States
{
    public sealed class CoolDownState : KitPackState
    {
        public CoolDownState(KitPackContext context, KitPackStateMachine.EKitPackState key) : base(context, key)
        {
        }

        public override void EnterState()
        {
            Context.CurrentTime = 0;
            Context.KitPackModel.SetActive(false);
            Context.ActionModule?.ExecuteAction(null);
        }

        public override void ExitState()
        {

        }

        public override void UpdateState(float deltaTime)
        {
            
        }
    }
}