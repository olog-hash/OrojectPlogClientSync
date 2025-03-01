using System;

namespace ProjectOlog.Code.Engine.StateMachines.Core
{
    public abstract class BaseState
    {
        public BaseState(Enum key)
        {
            StateKey = key;
        }
        
        public Enum StateKey { get; private set; }
        
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState(float deltaTime);
        public abstract Enum GetNextState();
    }
}