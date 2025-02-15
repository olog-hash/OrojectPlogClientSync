using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectOlog.Code.Game.StateMachines.Core
{
    public abstract class StateManager : MonoBehaviour
    {
        public BaseState CurrentState { get; protected set; }
        
        protected Dictionary<Enum, BaseState> States = new Dictionary<Enum, BaseState>();
        protected bool IsTransitioningState = false;

        public void OnLogicUpdate(float deltaTime)
        {
            if (IsTransitioningState || CurrentState == null) return;
            
            Enum nextStateKey = CurrentState.GetNextState();
            
            if (nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.UpdateState(deltaTime);
            }
            else
            {
                TransitionToState(nextStateKey);
            }
        }

        protected void TransitionToState(Enum nextStateKey)
        {
            IsTransitioningState = true;
            CurrentState?.ExitState();
            CurrentState = States[nextStateKey];
            PostTransition();
            CurrentState.EnterState();
            IsTransitioningState = false;
        }

        protected virtual void PostTransition()
        {
            
        }
    }
}