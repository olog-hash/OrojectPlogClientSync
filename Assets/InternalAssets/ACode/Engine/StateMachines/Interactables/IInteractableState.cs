using LiteNetLib.Utils;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Game.StateMachines.Interactables
{
    public interface IInteractableState
    {
        void Interact(EntityProvider entityProvider, NetDataPackage detail);
    }
}