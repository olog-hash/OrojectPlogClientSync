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
        private LayersManager _layersManager;

        [Inject]
        public BattleLevelState(UserConnectionNetworker userConnectionNetworker, LayersManager layersManager)
        {
            _userConnectionNetworker = userConnectionNetworker;
            _layersManager = layersManager;
        }

        public override void Enter()
        {
            // Выполняем загрузку и переключения слоя.
            SceneManager.LoadScene(1);
            
            _layersManager.ShowLayer("Gameplay");
            
            _userConnectionNetworker.ServerInitializeRequest();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            Debug.Log("BattleLevelState was completed.");
        }
    }
}