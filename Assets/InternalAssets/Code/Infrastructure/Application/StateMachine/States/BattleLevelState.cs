﻿using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine.States
{
    
    public class BattleLevelState : ApplicationState
    {
        private IClientSender _clientSender;
        private RuntimeHelper _runtimeHelper;
        private ApplicationStateMachine _applicationStateMachine;
        private UserConnectionNetworker _userConnectionNetworker;

        [Inject]
        public BattleLevelState(RuntimeHelper runtimeHelper, ApplicationStateMachine applicationStateMachine, IClientSender clientSender, UserConnectionNetworker userConnectionNetworker)
        {
            _runtimeHelper = runtimeHelper;
            _applicationStateMachine = applicationStateMachine;
            _clientSender = clientSender;
            _userConnectionNetworker = userConnectionNetworker;
        }

        public override void Enter()
        {
            // Очищаем все необходимые данные
            //BattleHUDSession.Initialize();

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