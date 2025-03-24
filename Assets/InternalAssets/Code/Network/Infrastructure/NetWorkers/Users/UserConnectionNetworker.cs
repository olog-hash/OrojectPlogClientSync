using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Features.Objects.Instantiate;
using ProjectOlog.Code.Features.Players.Instantiate;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets;
using ProjectOlog.Code.Network.Packets.SubPackets.Users;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.Network.Serialization.MetaData.Users;
using ProjectOlog.Code.UI.HUD;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users
{
    public sealed class UserConnectionNetworker : NetWorkerClient
    {
        private InstantiateUserUnpacker _instantiateUserUnpacker = new();

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
            if (serverInitializedCached.InitUsers.UserDataPackets.Length > 0)
            {
                var usersArray = _instantiateUserUnpacker.UnpackInstantiateUsersPacket(serverInitializedCached.InitUsers);

                for (int i = 0; i < usersArray.Length; i++)
                {
                    _usersContainer.AddUser(usersArray[i]);
                }
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
            var initUsersPacket = new InstantiateUserPacket();
            initUsersPacket.Deserialize(dataPackage);

            // Заполняем мета-информацию о пользователях на сервере
            if (initUsersPacket.UserDataPackets.Length > 0)
            {
                var usersArray = _instantiateUserUnpacker.UnpackInstantiateUsersPacket(initUsersPacket);

                for (int i = 0; i < usersArray.Length; i++)
                {
                    _usersContainer.AddUser(usersArray[i]);
                    
                    NotificationUtilits.ProcessNoneMessage($"Игрок {usersArray[i].Username} зашел на сервер!");
                }
            }
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