using ProjectOlog.Code.Networking.Profiles.Users;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    public class MainMenuState : ApplicationState
    {
        private DiContainer _container;
        private ApplicationStateMachine _applicationStateMachine;

        private NetworkUsersContainer _networkUsersContainer;

        [Inject]
        public MainMenuState(DiContainer container, ApplicationStateMachine applicationStateMachine)
        {
            _container = container;
            _applicationStateMachine = applicationStateMachine;
        }

        public override void Enter()
        {
            _networkUsersContainer = _container.Resolve<NetworkUsersContainer>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
    }
}