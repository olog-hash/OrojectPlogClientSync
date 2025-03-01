using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.UI.HUD;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users
{
    public sealed class UserChatNetworker : NetWorkerClient
    {
        public void SendMessageRequest(string message, ENetworkChatMessageType messageType = 0, byte receivedID = 0)
        {
            var dataPackage = new NetDataPackage(receivedID, (byte)messageType, message);
            
            SendTo(nameof(SendMessageRequest), dataPackage, DeliveryMethod.ReliableOrdered);
        }

        [NetworkCallback]
        public void SendMessageReceived(NetPeer peer, NetDataPackage dataPackage)
        {
            byte fromUserID = dataPackage.GetByte();
            var messageType = (ENetworkChatMessageType)dataPackage.GetByte();
            string message = dataPackage.GetString();

            var userData = _usersContainer.GetUserDataByID(fromUserID);
            string userName = userData is not null ? userData.Username : "NONE";

            switch (messageType)
            {
                case ENetworkChatMessageType.None:
                    NotificationUtilits.SendChatMessageNone(message);
                    break;
                case ENetworkChatMessageType.System:
                    NotificationUtilits.SendChatMessageSystem(message);
                    break;
                case ENetworkChatMessageType.User:
                    NotificationUtilits.SendChatMessagePlayer(userName, message);
                    break;
                default:
                    break;
            }
        }
    }
}