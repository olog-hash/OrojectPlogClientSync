using LiteNetLib.Utils;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables
{
    public interface IInteractableState
    {
        void Interact(EntityProvider entityProvider, NetDataPackage detail);
    }
}