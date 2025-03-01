using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interactables.StateMachines.KitPack
{
    public abstract class AbstractModuleAction : MonoBehaviour
    {
        public abstract void ExecuteAction(EntityProvider entityProvider);
    }
}