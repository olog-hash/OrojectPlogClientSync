using UnityEngine;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables
{
    public interface ITriggerState
    {
        public void HandleTriggerLogic(Collider triggerCollider, Collider[] collidersInside);
    }
}