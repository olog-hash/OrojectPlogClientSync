using ProjectOlog.Code.Infrastructure.Logging;
using ProjectOlog.Code.Network.Client;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    /// <summary>
    /// 2. Запуск сетевого состояния.
    /// </summary>
    public class NetworkLunchState : ApplicationState
    {
        private ApplicationStateMachine _applicationStateMachine;
        private NetworkClient _networkClient;

        [Inject]
        public NetworkLunchState(ApplicationStateMachine applicationStateMachine, NetworkClient networkClient)
        {
            _applicationStateMachine = applicationStateMachine;
            _networkClient = networkClient;
        }

        public override void Enter()
        {
            // По идее должен отвечать за привентивную инициализацию подключения к серверу
            _networkClient.StartNetwork();
            
            _applicationStateMachine.Enter<MainMenuState>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            Console.Log("NetworkLunch was completed.");
        }
    }
}