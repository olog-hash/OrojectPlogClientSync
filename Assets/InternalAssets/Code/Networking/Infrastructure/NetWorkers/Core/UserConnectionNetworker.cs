using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Players.Init;
using ProjectOlog.Code.Entities.Objects.Initialization;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets;
using ProjectOlog.Code.Networking.Profiles.Users;
using ProjectOlog.Code.UI.HUD;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core
{
    public sealed class UserConnectionNetworker : NetWorkerClient
    {
        public void ServerInitializeRequest()
        {
            var dataPackage = new NetDataPackage();
            
            SendTo(nameof(ServerInitializeRequest), dataPackage, DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void ServerInitializeReceive(NetPeer peer, NetDataPackage dataPackage)
        {  
            var serverInitializedCached = new ServerInitializedPacket();
            serverInitializedCached.Deserialize(dataPackage);
            
            // Заполняем мета-информацию о пользователях на сервере
            foreach (var userData in serverInitializedCached.InitUsers)
            {
                var user = new NetworkUserData()
                {
                    UserID = userData.UserID,
                    Username = userData.Username,
                    DeathCount = userData.DeathCount,
                    IsDead = userData.IsDead
                };

                _usersContainer.AddUser(user);
            }
            
            // Спавним пользователей что имеются.
            foreach (var initPlayerData in serverInitializedCached.InitPlayers)
            {
                var userData = _usersContainer.GetUserDataByID(initPlayerData.UserID);
  
                var initPlayerEvent = new InitPlayerEvent 
                {
                    ServerID = initPlayerData.ServerID,
                    UserID = initPlayerData.UserID,
                    Username = (userData?.Username != null ? userData.Username : "NONE"),

                    Position = initPlayerData.Position,
                    Rotation = initPlayerData.Rotation,
                    IsDead = initPlayerData.IsDead,
                };
                
                World.Default.CreateTickEvent().AddComponentData(initPlayerEvent);
            }

            // Спавним обьекты, что имеются
            foreach (var initObjectData in serverInitializedCached.InitObjects)
            {
               var initObjectEvent = new InitObjectEvent
                {
                    ServerID = initObjectData.ServerID,
                    ObjectType = initObjectData.ObjectType,

                    Position = initObjectData.Position,
                    Rotation = initObjectData.Rotation,
                    
                    ObjectData = initObjectData.ObjectData,
                };

                World.Default.CreateTickEvent().AddComponentData(initObjectEvent);
            }

            NotificationUtilits.SendChatMessageNone($"Добро пожаловать на сервер!");
        }

        [NetworkCallback]
        public void OnUserConnected(NetPeer peer, NetDataPackage dataPackage)
        {
            var userDataCached = new InitUserPacket();
            userDataCached.Deserialize(dataPackage);

            _usersContainer.AddUser(new NetworkUserData()
            {
                UserID = userDataCached.UserID,
                Username = userDataCached.Username,
            });

            NotificationUtilits.SendChatMessageNone($"Игрок {userDataCached.Username} зашел на сервер!");
        }
        
        [NetworkCallback]
        public void OnUserDisconnected(NetPeer peer, NetDataPackage dataPackage)
        {
            byte userID = dataPackage.GetByte();
            
            if (_usersContainer.TryGetUserDataByID(userID, out var user))
            { 
                _usersContainer.RemoveUser(userID);
                
                NotificationUtilits.SendChatMessageNone($"Игрок {user.Username} покинул сервер!");
            }
        }
    }
}