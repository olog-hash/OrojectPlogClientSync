using LiteNetLib.Utils;

namespace ProjectOlog.Code.Game.StateMachines.Interactables.Networking
{
    public interface IObjectNetworkerContainer
    {
        public void HandleRequest(NetDataPackage dataPackage);
    }
}