namespace ProjectOlog.Code.Infrastructure.Application.StateMachine
{
    public abstract class ApplicationState
    {
        public abstract void Enter();
        public abstract void OnUpdate(float deltaTime);
        public abstract void Exit();
    }
}