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
        
        // Обработка получение глобального сообщения от сервера
        [NetworkCallback]
        private void SendServerMessageForAll(NetPeer peer, NetDataPackage dataPackage)
        {
            var messageType = (ENetworkChatMessageType)dataPackage.GetByte();
            string messageText = dataPackage.GetString();

            NotificationUtilits.ProcessServerMessage(messageType, messageText);
        }
        
        // Обработка получения личного сообщения от сервера
        [NetworkCallback]
        private void SendServerMessageFor(NetPeer peer, NetDataPackage dataPackage)
        {
            var messageType = (ENetworkChatMessageType)dataPackage.GetByte();
            string messageText = dataPackage.GetString();

            NotificationUtilits.ProcessServerMessage(messageType, messageText);
        }
        
        // Обработка получение глобального сообщения от игрока.
        [NetworkCallback]
        public void SendPlayerMessageForAll(NetPeer peer, NetDataPackage dataPackage)
        {
            var messageType = (ENetworkChatMessageType)dataPackage.GetByte();
            byte fromUserID = dataPackage.GetByte();
            string messageText = dataPackage.GetString();

            if (!_usersContainer.TryGetUserDataByID(fromUserID, out var userData)) return;
            
            NotificationUtilits.ProcessPlayerMessage(userData.Username, messageText);
        }
        
        // Обработка получение личного сообщения от игрока.
        [NetworkCallback]
        public void SendPlayerMessageFor(NetPeer peer, NetDataPackage dataPackage)
        {
            var messageType = (ENetworkChatMessageType)dataPackage.GetByte();
            byte fromUserID = dataPackage.GetByte();
            byte toUserID = dataPackage.GetByte();
            string messageText = dataPackage.GetString();

            if (!_usersContainer.TryGetUserDataByID(fromUserID, out var userData)) return;
            
            NotificationUtilits.ProcessPlayerMessage(userData.Username, messageText);
        }
    }
}