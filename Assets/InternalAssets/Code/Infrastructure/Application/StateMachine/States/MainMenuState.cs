using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Profiles.Users;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    /// <summary>
    /// 3. Состояние игрового меню
    /// </summary>
    public class MainMenuState : ApplicationState
    {
        private ApplicationStateMachine _applicationStateMachine;

        private LayersManager _layersManager;
        
        [Inject]
        public MainMenuState(ApplicationStateMachine applicationStateMachine, LayersManager layersManager)
        {
            _applicationStateMachine = applicationStateMachine;
            _layersManager = layersManager;
        }

        public override void Enter()
        {
            _layersManager.ShowLayer("MainMenu");
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
    }
}