using System;
using LiteNetLib.Utils;
using ProjectOlog.Code.Engine.StateMachines.Core;
using ProjectOlog.Code.Engine.StateMachines.Interactables.Networking;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables
{
    public abstract class InteractionObjectStateManager : StateManager
    {
        protected abstract Type EnumType { get; }
        public string InteractionObjectName { get; protected set; }
        public string InteractionObjectDescription { get; protected set; }
        
        public EntityProvider InteractiveObjectEntityProvider => _interactiveObjectEntityProvider;
        public IObjectNetworkerContainer ObjectNetworkerContainer => _objectNetworkerContainer;
        
        [Header("Interactive Object EntityProvider")]
        [SerializeField]
        protected EntityProvider _interactiveObjectEntityProvider;
        protected IObjectNetworkerContainer _objectNetworkerContainer;
        
        public abstract void Init();
        public abstract void SetSnapshotData(NetDataPackage dataPackage);
        public abstract NetDataPackage GetSnapshotData();

        protected void RegisterNetworker(ObjectNetworker objectNetworker)
        {
            _objectNetworkerContainer = new ObjectNetworkerContainer(objectNetworker);
        }

        public void TransitionState(Enum newState, bool immediately = false)
        {
            if (immediately)
            {
                CurrentState = States[newState];
            }
            else
            {
                TransitionToState(newState);
            }
        }

        protected override void PostTransition()
        {
            if (CurrentState is InteractionObjectState interactionState)
            {
                // Обнуляем ссылку на следующее состояние у текущего.
                interactionState.ResetNextState();
            }
        }
        
        public bool IsAvaliableToInteract()
        {
            if (CurrentState is null) return false;
            
            return CurrentState is IInteractableState;
        }
        
        public void InteractWithObject(EntityProvider entityProvider, NetDataPackage detail)
        {
            if (CurrentState is IInteractableState interactableState)
            {
                interactableState.Interact(entityProvider, detail);
            }
            else
            {
                Debug.LogWarning("Current state does not support interaction.");
            }
        }
        
        public string GetStateInteractName()
        {
            if (CurrentState is null) return string.Empty;

            if (CurrentState is InteractionObjectState interactionObjectState)
            {
                if (string.IsNullOrEmpty(interactionObjectState.InteractionStateName))
                {
                    return "ВЗАИМОДЕЙСТВИЕ";
                }
                
                return interactionObjectState.InteractionStateName;
            }

            return string.Empty;
        }
        
        public Enum GetEnumFromInt(int value)
        {
            return (Enum)Enum.ToObject(EnumType, value);
        }
    }
}