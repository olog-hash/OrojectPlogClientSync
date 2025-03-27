using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Network.Client;
using ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    /// <summary>
    /// 4. Загрузка сцены
    /// </summary>
    public class BattleLevelState : ApplicationState
    {
        private UserConnectionNetworker _userConnectionNetworker;

        [Inject]
        public BattleLevelState(UserConnectionNetworker userConnectionNetworker)
        {
            _userConnectionNetworker = userConnectionNetworker;
        }

        public override void Enter()
        {
            // Выполняем загрузку и переключения слоя.
            SceneManager.LoadScene(1);
            LayersManager.ShowLayer("Gameplay");
            
            _userConnectionNetworker.ServerInitializeRequest();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            Debug.Log("Bootstrap was completed.");
        }
    }
}