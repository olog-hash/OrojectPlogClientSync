using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core
{
    public sealed class UserRegistrationNetworker : NetWorkerClient
    {
        private IClientConnector _clientConnector;

        public UserRegistrationNetworker(IClientConnector clientConnector)
        {
            _clientConnector = clientConnector;
        }

        public void UserRegistrationRequest(string username)
        {
            var sendDataPackage = new NetDataPackage(username);
            
            SendTo(nameof(UserRegistrationRequest), sendDataPackage, DeliveryMethod.ReliableOrdered);
        }
        

        [NetworkCallback]
        public void UserRegistrationRequest(NetPeer peer, NetDataPackage dataPackage)
        {
            bool accept = dataPackage.GetBool();
            string reason = dataPackage.GetString();
            
            if (accept)
            {
                int userID = dataPackage.GetInt();
                string userName = dataPackage.GetString();
                
                LocalData.LocalID = userID;
                
                Debug.Log($"Запрос на регистрацию прошел успешно {userID} {userName}");
            }
            else
            {
                // Мы получили отказ на регистрацию
                Debug.Log($"Запрос на регистрацию был отклонен... {reason}");
            }
            
            _clientConnector.ConnectedResult(accept, reason);
            
            Debug.Log($"Room register pass: {accept} {reason}");
        }
    }
}