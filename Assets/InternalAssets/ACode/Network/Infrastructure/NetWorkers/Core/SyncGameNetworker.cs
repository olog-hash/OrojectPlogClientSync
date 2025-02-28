using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Game.Snapshot.Receive;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets;
using ProjectOlog.Code.Networking.Packets.SystemSync.Receive;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core
{
    public sealed class SyncGameNetworker : NetWorkerClient
    {
        public void SyncPlayerRequest(NetDataPackage dataPackage)
        {
            SendTo(nameof(SyncPlayerRequest), dataPackage, DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void SyncGameSnapshot(NetPeer peer, NetDataPackage dataPackage)
        {
            var snapshotPacket = new ServerSnapshotPacket();
            snapshotPacket.Deserialize(dataPackage);

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