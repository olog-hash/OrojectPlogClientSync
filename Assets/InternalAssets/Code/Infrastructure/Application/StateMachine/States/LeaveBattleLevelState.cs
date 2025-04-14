using ProjectOlog.Code.Context;
using ProjectOlog.Code.Network.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    public class LeaveBattleLevelState : ApplicationState
    {
        private ApplicationStateMachine _applicationStateMachine;
        private ContextLifeCycleService _contextLifeCycleService;
        private NetworkClient _networkClient;
        
        public LeaveBattleLevelState(ContextLifeCycleService contextLifeCycleService, NetworkClient networkClient, ApplicationStateMachine applicationStateMachine)
        {
            _contextLifeCycleService = contextLifeCycleService;
            _networkClient = networkClient;
            _applicationStateMachine = applicationStateMachine;
        }

        public override void Enter()
        {
            _networkClient.StopNetwork();
            
            // Проводим очистку данных в основных сервисах игры
            _contextLifeCycleService.ResetSceneContext();
            
            // Выполняем загрузку и переключения слоя.
            SceneManager.LoadScene(0);
            
            _applicationStateMachine.Enter<NetworkLunchState>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            Debug.Log("LeaveBattleLevel was completed.");
        }
    }
}