using System;
using System.Collections.Generic;
using ProjectOlog.Code.Infrastructure.Application.StateMachine.States;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.Application.StateMachine
{
    public class ApplicationStateMachine: IInitializable, IUpdate
    {
        private readonly ApplicationStateFactory _stateFactory;

        private Dictionary<Type, ApplicationState> _states;
        private ApplicationState _currentState;
        
        public ApplicationStateMachine(ApplicationStateFactory stateFactory, RuntimeHelper runtimeHelper)
        {
            runtimeHelper.RegisterUpdate(this);

            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            _states = new Dictionary<Type, ApplicationState>
            {
                [typeof(BootstrapState)] = _stateFactory.CreateState<BootstrapState>(),
                [typeof(NetworkLunchState)] = _stateFactory.CreateState<NetworkLunchState>(),
                [typeof(MainMenuState)] = _stateFactory.CreateState<MainMenuState>(),
                [typeof(BattleLevelState)] = _stateFactory.CreateState<BattleLevelState>(),
                [typeof(LeaveBattleLevelState)] = _stateFactory.CreateState<LeaveBattleLevelState>(),
            };
            
            Enter<BootstrapState>();
        }

        public void Enter<T>() where T : ApplicationState
        {
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState.Enter();
        }

        public void OnUpdate(float deltaTime)
        {
            _currentState?.OnUpdate(deltaTime);
        }
    }
}