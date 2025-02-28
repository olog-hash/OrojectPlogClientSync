using UnityEngine;

namespace ProjectOlog.Code.Game.StateMachines.Interactables
{
    public interface ITriggerState
    {
        public void HandleTriggerLogic(Collider triggerCollider, Collider[] collidersInside);
    }
}