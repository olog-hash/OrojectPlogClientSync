using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Features.Entities.Destruction.ListDestroy;
using ProjectOlog.Code.Features.Entities.Destruction.SingleDestroy;
using ProjectOlog.Code.Features.Objects.Instantiate;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Entities
{
    public sealed class ObjectNetworker : NetWorkerClient
    {
        
        public void SpawnObjectRequest(ENetworkObjectType objectType, Vector3 position, Quaternion rotation)
        {
            var dataPackage = new NetDataPackage((byte)objectType, position, rotation);
            
            SendTo(nameof(SpawnObjectRequest), dataPackage, DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void InitObject(NetPeer peer, NetDataPackage dataPackage)
        {
            var instantiateObjectPacket = new InstantiateObjectPacket();
            instantiateObjectPacket.Deserialize(dataPackage);

            if (instantiateObjectPacket.Length > 0)
            {
                World.Default.CreateTickEvent().AddComponentData(new InstantiateObjectEvent()
                {
                    InstantiateObjectPacket = instantiateObjectPacket,
                    EntityProviderMappingPool = new EntityProviderMappingPool()
                });
            }
        }
        
        [NetworkCallback]
        private void DestroyObject(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort destroyObjectServerID = dataPackage.GetUShort();
            
            var destroyNetworkObjectEvent = new DestroyNetworkObjectEvent()
            {
                ServerID = destroyObjectServerID,
            };
            
            World.Default.CreateTickEvent().AddComponentData(destroyNetworkObjectEvent);
        }
        
        [NetworkCallback]
        private void DestroyObjectsList(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort[] indestructibleObjectIds = dataPackage.GetUShortArray();
        
            var clientMassDestroyEvent = new DestroyObjectsListEvent
            {
                DestructibleObjectIds = indestructibleObjectIds
            };
        
            World.Default.CreateTickEvent().AddComponentData(clientMassDestroyEvent);
        }
    }
}