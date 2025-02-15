using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using ProjectOlog.Code.Networking.Infrastructure;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Networking.Client
{
    public class NetworkClient : MonoBehaviour, INetEventListener, IClientSender, IClientConnector, ITickUpdate, IUpdate, ILateUpdate
    {
        public NetStatistics NetStatistics => _netManager?.Statistics;
        
        // Инструменты
        private NetPeer _server;
        private NetManager _netManager;
        private NetDataWriter _writer;
        private NetworkClientGate _clientHandler;
        private RuntimeHelper _runtimeHelper;
        private NetTransportProvider _transportProvider;
        
        // Ивенты
        public event Action OnConnected;
        public event Action<string> OnDisconnected;

        [Inject]
        public void Construct(RuntimeHelper runtimeHelper, NetworkClientGate clientHandler, NetTransportProvider transportProvider)
        {
            NetworkTime.SetTickrate(NetworkTime.DEFAULT_TICK_RATE);
            NetworkTime.ClearPreviousData();
            
            _runtimeHelper = runtimeHelper;
            _transportProvider = transportProvider;
            _clientHandler = clientHandler;
            _writer = new NetDataWriter();
            
            _netManager = new NetManager(this)
            {
                AutoRecycle = true,
                EnableStatistics = true,
                IPv6Enabled = false,
                SimulateLatency = true,
                SimulationMinLatency = 50,
                SimulationMaxLatency = 60,
                SimulatePacketLoss = false,
                SimulationPacketLossChance = 15
            };
            _netManager.Start();

            _runtimeHelper.RegisterUpdate(this);
            _runtimeHelper.RegisterTickUpdate(this);
            _runtimeHelper.RegisterLateUpdate(this);
        }

        // Отправляем запрос на подключение к серверу
        public void ConnectToServer(string ip)
        {
            _netManager.Connect(ip, 10515, "ExampleGame");
        }

        // Тикрейт обновления
        public void OnTickUpdate(float deltaTime)
        {
            //NetworkTime.UpdateLocalTick(); -> Обновляется в RuntimeHelper

            _clientHandler.OnLogicUpdate(deltaTime);
            
            _transportProvider.ProcessBatchQueue();
        }
        
        // Цикл для мониторинга поступающих событий
        public void OnUpdate(float deltaTime)
        {
            _netManager.PollEvents();
        }
        
        public void OnLateUpdate(float deltaTime)
        {
            _transportProvider.ProcessBatchQueue();
        }
        
        // Получаем задержку со стороны сервера (наш пинг)
        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            NetworkTime.UpdatePing(latency);
        }

        // Отклоняем внешние подключения в нашу сторону
        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Reject();
        }

        // Получаем пакеты со стороны сервера
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            _clientHandler.OnNetworkReceive(peer, reader, channelNumber, deliveryMethod);
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Debug.Log("[C] Connected to server: " + peer);

            NetworkTime.ClearPreviousData();

            _server = peer;
            _clientHandler.OnPeerConnected(peer);
            OnConnected?.Invoke();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Debug.Log("[C] Disconnected from server: " + disconnectInfo.Reason);

            NetworkTime.ClearPreviousData();
            _clientHandler.OnPeerDisconnected(peer, disconnectInfo);
            OnDisconnected?.Invoke(disconnectInfo.Reason.ToString());
        }
        
        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Debug.Log("[C] NetworkError: " + socketError);
        }
        
        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        private void OnDestroy()
        {
            _netManager.Stop();
        }
        
        // ========== Инструментарий ==========
        
        public void ConnectedResult(bool success, string reason)
        {
            if (success)
            {
                OnConnected?.Invoke();
            }
            else
            {
                OnDisconnected?.Invoke(reason);
            }
        }
        
        public NetDataWriter WritePacket(byte[] dataArray)
        {
            _writer.Reset();
            _writer.Put(dataArray.Length);
            _writer.Put(dataArray);
            
            return _writer;
        }

        public void Send(NetDataWriter writer, DeliveryMethod deliveryMethod)
        {
            if (_server == null)
                return;
            
            _server.Send(writer, deliveryMethod);
        }
    }
}