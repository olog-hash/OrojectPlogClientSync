using ProjectOlog.Code.Context;
using ProjectOlog.Code.DataStorage.Core;
using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.Logging;
using ProjectOlog.Code.Infrastructure.ResourceManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Network.Client;
using UnityEngine;
using Zenject;
using Screen = UnityEngine.Device.Screen;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    /// <summary>
    ///  1. Начальное состояние приложения.
    /// </summary>
    public class BootstrapState: ApplicationState
    {
        private ApplicationStateMachine _applicationStateMachine;

        private ContextLifeCycleService _contextLifeCycleService;

        [Inject]
        public BootstrapState(ApplicationStateMachine applicationStateMachine, ContextLifeCycleService contextLifeCycleService)
        {
            _applicationStateMachine = applicationStateMachine;
            _contextLifeCycleService = contextLifeCycleService;
        }
        
        public override void Enter()
        {
            // Делаем lock для частоты кадров, ограничивая её способностью монитора.
            LockRefreshRate();

            // Проводим очистку данных в основных сервисах игры
            _contextLifeCycleService.ResetProjectContext();
            
            _applicationStateMachine.Enter<NetworkLunchState>();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void Exit()
        {
            Console.Log("Bootstrap was completed.");
        }

        private void LockRefreshRate()
        {
            // Получение частоты обновления монитора (примерно)
            int refreshRate = Screen.currentResolution.refreshRate;
            
            // Установка ограничения FPS
            UnityEngine.Device.Application.targetFrameRate = refreshRate;

            Debug.Log($"Включено ограничение FPS : {refreshRate}");
        }
    }
}