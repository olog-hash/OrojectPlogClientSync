using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code._InDevs.Players.Instantiate;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Networking.Packets;
using ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Players
{
    public sealed class PlayerNetworker : NetWorkerClient
    {
        public void SpawnPlayerRequest()
        {
            SendTo(nameof(SpawnPlayerRequest), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void InitPlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            var instantiatePlayerPacket = new InstantiatePlayerPacket();
            instantiatePlayerPacket.Deserialize(dataPackage);
            
            if (instantiatePlayerPacket.NetworkPlayerDatas.Length > 0)
            {
                World.Default.CreateTickEvent().AddComponentData(new InstantiatePlayerEvent()
                {
                    InstantiatePlayerPacket = instantiatePlayerPacket,
                    EntityProviderMappingPool = new EntityProviderMappingPool()
                });
            }
        }
        
        [NetworkCallback]
        private void RespawnPlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            byte userID = dataPackage.GetByte();
            ushort stateVersion = dataPackage.GetUShort();
            var position = dataPackage.GetVector3();
            var rotation = dataPackage.GetVector4();

            if (!_entitiesContainer.PlayerEntities.TryGetPlayerEntity(userID, out var playerProvider)) return;
            
            var respawnPlayerEvent = new RespawnPlayerEvent
            {
                PlayerProvider = playerProvider,
                LastStateVersion = stateVersion,
                Position = position,
                Rotation = rotation,
            };
            
            World.Default.CreateTickEvent().AddComponentData(respawnPlayerEvent);
        }
    }
}