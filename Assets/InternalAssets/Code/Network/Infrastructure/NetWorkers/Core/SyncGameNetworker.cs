using System;
using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Client;
using ProjectOlog.Code.Network.Gameplay.Snapshot.Receive;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.Core.Compressors;
using ProjectOlog.Code.Network.Packets.SystemSync.Receive;
using ProjectOlog.Code.Network.Packets.SystemSync.Send;
using ProjectOlog.Code.Network.Profiles.Snapshots.Fragmentation;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Core
{
    public sealed class SyncGameNetworker : NetWorkerClient
    {
        private SnapshotFragmentCollector _fragmentCollector = new SnapshotFragmentCollector();

        public void SyncPlayerRequest(PlayerStateClientPacket playerStateClientPacket)
        {
            SendTo(nameof(SyncPlayerRequest), playerStateClientPacket.GetPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        // Отправляем запрос на желание получения глобального снапшота для лучшей синхронизации
        public void GlobalSnapshotRequest()
        {
            SendTo(nameof(SyncPlayerRequest), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void SnapshotSync(NetPeer peer, NetDataPackage dataPackage)
        {
            // Десериализуем фрагмент
            var fragment = new SnapshotFragment();
            fragment.Deserialize(dataPackage);
    
            // Добавляем фрагмент и обновляем менеджер
            _fragmentCollector.AddFragment(fragment);
    
            // Проверяем готовность снапшота
            if (_fragmentCollector.IsSnapshotComplete(fragment.SnapshotId))
            {
                // Получаем полный сжатый снапшот
                byte[] completeData = _fragmentCollector.GetCompleteSnapshot(fragment.SnapshotId);
        
                // Преобразуем в формат NetDataPackage
                NetDataPackage compressedPackage = new NetDataPackage();
                compressedPackage.FromData(completeData);
        
                // Распаковываем пакет
                NetDataPackage decompressedPackage = LZ4PackageCompressor.DecompressPackage(compressedPackage);
        
                // Проверяем, успешно ли прошла распаковка
                if (decompressedPackage == null) {
                    Debug.LogError("Не удалось распаковать снапшот");
                    return;
                }
        
                // Десериализуем снапшот из распакованного пакета
                ServerSnapshotPacket snapshotPacket = new ServerSnapshotPacket();
                snapshotPacket.Deserialize(decompressedPackage);
        
                // Дальнейшая обработка снапшота
                var serverSnapshotEvent = new ServerSnapshotEvent 
                {
                    LastServerTick = snapshotPacket.LastServerTick,
                    LastServerTime = snapshotPacket.LastServerTime,
                    LastClientReceivedTick = snapshotPacket.LastClientReceivedTick,
                    BroadcastType = snapshotPacket.BroadcastType,
                    
                    PlayersTransform = snapshotPacket.PlayersData.ToArray(),
                    ObjectsTransform = snapshotPacket.ObjectsData.ToArray(),
                };
        
                World.Default.CreateTickEvent().AddComponentData(serverSnapshotEvent);
            }
        }
        
        // Получаем ПОЛНЫЙ и ГАРАНТИРОВАННЫЙ снапшот со стороны сервера
        [NetworkCallback]
        private void ReliableSnapshotSync(NetPeer peer, NetDataPackage dataPackage)
        {
            /*
            // Распаковываем пакет
            NetDataPackage decompressedPackage = LZ4PackageCompressor.DecompressPackage(dataPackage);
            
            // Проверяем, успешно ли прошла распаковка
            if (decompressedPackage == null) {
                Debug.LogError("Не удалось распаковать снапшот");
                return;
            }*/
            
            // Десериализуем снапшот из распакованного пакета
            ServerSnapshotPacket snapshotPacket = new ServerSnapshotPacket();
            snapshotPacket.Deserialize(dataPackage);

            Debug.Log($"Получен глобальный снапшот в виде {snapshotPacket.LastServerTick}");
            
            // Дальнейшая обработка снапшота
            var serverSnapshotEvent = new ServerSnapshotEvent 
            {
                LastServerTick = snapshotPacket.LastServerTick,
                LastServerTime = snapshotPacket.LastServerTime,
                LastClientReceivedTick = snapshotPacket.LastClientReceivedTick,
                BroadcastType = snapshotPacket.BroadcastType,
                    
                PlayersTransform = snapshotPacket.PlayersData.ToArray(),
                ObjectsTransform = snapshotPacket.ObjectsData.ToArray(),
            };
        
            World.Default.CreateTickEvent().AddComponentData(serverSnapshotEvent);
        }
    }
}