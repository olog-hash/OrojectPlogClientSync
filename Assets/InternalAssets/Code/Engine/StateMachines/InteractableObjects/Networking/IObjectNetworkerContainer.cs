using LiteNetLib.Utils;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables.Networking
{
    public interface IObjectNetworkerContainer
    {
        public void HandleRequest(NetDataPackage dataPackage);
    }
}