using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users;
using ProjectOlog.Code.Network.Profiles.Entities;
using ProjectOlog.Code.Network.Profiles.Snapshots;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.HUD;
using UnityEngine;
using Zenject;
using PrimitiveType = LiteNetLib.Utils.PrimitiveType;

namespace ProjectOlog.Code.Network.Client
{
    public class NetworkClientGate
    {
        private IClientSender _clientSender;
        private List<NetWorkerContainer> _netWorkerContainers;
        private NetWorkerFactory _netWorkerFactory;

        // Инструменты
        private NetworkUsersContainer _usersContainer;
        private NetworkEntitiesContainer _entitiesContainer;
        private NetworkSnapshotContainer _snapshotContainer;
        

        [Inject]
        public void Construct(IClientSender clientSender, NetWorkerFactory netWorkerFactory, NetworkUsersContainer usersContainer, NetworkEntitiesContainer entitiesContainer, NetworkSnapshotContainer snapshotContainer)
        {
            _clientSender = clientSender;
            _netWorkerFactory = netWorkerFactory;
            _snapshotContainer = snapshotContainer;

            _usersContainer = usersContainer;
            _entitiesContainer = entitiesContainer;

            _netWorkerContainers = new List<NetWorkerContainer>();
            
            InitializeData();
            RegisterAllNetWorkers();
        }

        private void InitializeData()
        {
            _usersContainer.ClearContainer();
            _entitiesContainer.ClearContainer();
            _snapshotContainer.ClearContainer();
        }
        
        private void RegisterAllNetWorkers()
        {
            var netWorkers = _netWorkerFactory.GetAllNetWorkers();

            foreach (var netWorker in netWorkers)
            {
                RegisterNetWorker(netWorker);
            }
            
            Debug.Log($"NetWorkers were initialized! ({netWorkers.Count}) {_netWorkerContainers.Count} ");
        }
        
        public void RegisterNetWorker(NetWorkerClient net)
        {
            var container = new NetWorkerContainer(net);
            container.Initialize();
            
            _netWorkerContainers.Add(container);
        }


        public void OnLogicUpdate(float deltaTime)
        {
            for (int i = 0; i < _netWorkerContainers.Count; i++)
            {
                _netWorkerContainers[i].NetWorker.OnLogicUpdate();
            }
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            if (reader.UserDataSize < 4) return;

            int dataLength = reader.GetInt();
            byte[] dataArray = new byte[dataLength];
            reader.GetBytes(dataArray, dataLength);
            
            var netDataPackage = new NetDataPackage();
            netDataPackage.FromData(dataArray);
            
            if (netDataPackage.CurrentType != PrimitiveType.String) return;
            string netWorkerName = netDataPackage.GetString();

            var netWorkerContainer = _netWorkerContainers.Find(x => x.NetWorker.Name == netWorkerName);
            if (netWorkerContainer == null) return;
            
            netWorkerContainer.HandleRequest(peer, netDataPackage);
        }

        public void OnPeerConnected(NetPeer peer)
        {
            // Тут ошибка - возвращает пустой нетворкер!!!
            var registrationNetworker = _netWorkerFactory.GetNetWorker<UserRegistrationNetworker>();
            registrationNetworker.UserRegistrationRequest(LocalData.LocalName);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            NotificationUtilits.SendChatMessageAlert("Подключение к серверу было разорвано!");
        }
    }
}