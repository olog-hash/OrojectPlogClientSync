using System;
using ProjectOlog.Code.Engine.StateMachines.Core;
using UnityEngine;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables
{
    public abstract class InteractionObjectState : BaseState
    {
        public string InteractionStateName { get; protected set; }
        public virtual bool SyncStateChange => true;
        
        protected Enum NextState; 
        
        public InteractionObjectState(Enum key) : base(key)
        {
            NextState = key;
        }

        public void ResetNextState()
        {
            NextState = StateKey;
        }

        protected void SetNextState(Enum state)
        {
            NextState = state;
        }

        public override Enum GetNextState()
        {
            return NextState;
        }
        
        // Метод для проверки и обработки триггера
        protected void CheckAndHandleTrigger(Collider triggerCollider)
        {
            if (triggerCollider == null)
            {
                Debug.LogError("Trigger collider is null");
                return;
            }

            if (!triggerCollider.isTrigger)
            {
                Debug.LogWarning("The provided collider is not set as a trigger. Results may be unexpected.");
            }

            // Получаем все коллайдеры внутри триггера
            Collider[] collidersInside = Physics.OverlapBox(
                triggerCollider.bounds.center,
                triggerCollider.bounds.extents,
                triggerCollider.transform.rotation
            );

            // Вызываем абстрактный метод для обработки логики
            if (this is ITriggerState triggerEvent)
            {
                triggerEvent.HandleTriggerLogic(triggerCollider, collidersInside);
            }
        }

        // Вспомогательный метод для проверки, находится ли объект внутри триггера
        protected bool IsInsideTrigger(Collider objectCollider, Collider triggerCollider)
        {
            return triggerCollider.bounds.Intersects(objectCollider.bounds);
        }
    }
}