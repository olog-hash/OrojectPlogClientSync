using ProjectOlog.Code.Network.Profiles.Users;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    /// <summary>
    /// 3. Состояние игрового меню
    /// </summary>
    public class MainMenuState : ApplicationState
    {
        private DiContainer _container;
        private ApplicationStateMachine _applicationStateMachine;

        [Inject]
        public MainMenuState(DiContainer container, ApplicationStateMachine applicationStateMachine)
        {
            _container = container;
            _applicationStateMachine = applicationStateMachine;
        }

        public override void Enter()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
    }
}