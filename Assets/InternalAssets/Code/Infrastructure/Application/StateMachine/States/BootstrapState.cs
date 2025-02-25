using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.Logging;
using ProjectOlog.Code.Infrastructure.ResourceManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Input.Controls;
using ProjectOlog.Code.Networking.Client;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    public class BootstrapState: ApplicationState
    {
        private RuntimeHelper _runtimeHelper;
        private ApplicationStateMachine _applicationStateMachine;
        
        private ApplicationLayersController _layersController;

        [Inject]
        public BootstrapState(RuntimeHelper runtimeHelper, ApplicationStateMachine applicationStateMachine) 
        {
            _runtimeHelper = runtimeHelper;
            _applicationStateMachine = applicationStateMachine;
        }
        
        public override void Enter()
        {
            //UnityEngine.Application.targetFrameRate = 60;
            
            // Initialization
            _layersController = new ApplicationLayersController(_runtimeHelper);
            
            InputControls.Reset();
            
            LayersManager.Reset();
            LayersManager.ShowLayer("MainMenu");

            PrefabsResourcesLoader.Reset();
            PrefabsResourcesLoader.RegisterAllPrefabs("Prefabs");
            
            NetworkTime.Reset();
            NetworkObjectRegistration.RegisterNetworkObjects();
            
            Console.Log("Bootstrap was completed.");
            _applicationStateMachine.Enter<NetworkLunchState>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {

        }
    }
}