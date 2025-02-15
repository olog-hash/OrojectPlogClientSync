using ProjectOlog.Code.Infrastructure.Logging;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    public class NetworkLunchState : ApplicationState
    {
        private DiContainer _container;
        private ApplicationStateMachine _applicationStateMachine;

        [Inject]
        public NetworkLunchState(DiContainer container, ApplicationStateMachine applicationStateMachine)
        {
            _container = container;
            _applicationStateMachine = applicationStateMachine;
        }

        public override void Enter()
        {
            // По идее должен отвечать за привентивную инициализацию подключения к серверу... но т.к такие возможности еще нет - пустышка.
            
            Console.Log("NetworkLunch was completed.");
            
            _applicationStateMachine.Enter<MainMenuState>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            
        }
    }
}