﻿using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Features.Objects.Instantiate;
using ProjectOlog.Code.Features.Players.Instantiate;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.HUD;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users
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
                var user = new NetworkUserData(userData.UserID, userData.Username);
                user.GameState.Deaths.Value = userData.DeathCount;
                user.GameState.IsDead.Value = userData.IsDead;

                _usersContainer.AddUser(user);
            }
            
            // Спавним пользователей что имеются.
            if (serverInitializedCached.InstantiatePlayerPacket.Length > 0)
            {
                World.Default.CreateTickEvent().AddComponentData(new InstantiatePlayerEvent()
                {
                    InstantiatePlayerPacket = serverInitializedCached.InstantiatePlayerPacket,
                    EntityProviderMappingPool = new EntityProviderMappingPool()
                });
            }
            
            // Спавним обьекты, что имеются
            if (serverInitializedCached.InstantiateObjectPacket.Length > 0)
            {
                World.Default.CreateTickEvent().AddComponentData(new InstantiateObjectEvent()
                {
                    InstantiateObjectPacket = serverInitializedCached.InstantiateObjectPacket,
                    EntityProviderMappingPool = new EntityProviderMappingPool()
                });
            }

            NotificationUtilits.ProcessNoneMessage($"Добро пожаловать на сервер!");
        }

        [NetworkCallback]
        public void OnUserConnected(NetPeer peer, NetDataPackage dataPackage)
        {
            var userDataCached = new InitUserPacket();
            userDataCached.Deserialize(dataPackage);

            var user = new NetworkUserData(userDataCached.UserID, userDataCached.Username);
            _usersContainer.AddUser(user);

            NotificationUtilits.ProcessNoneMessage($"Игрок {userDataCached.Username} зашел на сервер!");
        }
        
        [NetworkCallback]
        public void OnUserDisconnected(NetPeer peer, NetDataPackage dataPackage)
        {
            byte userID = dataPackage.GetByte();
            
            if (_usersContainer.TryGetUserDataByID(userID, out var user))
            { 
                _usersContainer.RemoveUser(userID);
                
                NotificationUtilits.ProcessNoneMessage($"Игрок {user.Username} покинул сервер!");
            }
        }
    }
}