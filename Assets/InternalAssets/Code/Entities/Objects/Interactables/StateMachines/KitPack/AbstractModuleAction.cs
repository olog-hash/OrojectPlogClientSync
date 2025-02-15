using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.StateMachines.KitPack
{
    public abstract class AbstractModuleAction : MonoBehaviour
    {
        public abstract void ExecuteAction(EntityProvider entityProvider);
    }
}