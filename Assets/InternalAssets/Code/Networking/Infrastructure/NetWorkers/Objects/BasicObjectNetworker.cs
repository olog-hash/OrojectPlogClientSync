using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Entities.Objects.Destruction;
using ProjectOlog.Code.Entities.Objects.Instantiate;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Networking.Packets;
using ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Objects
{
    public sealed class BasicObjectNetworker : NetWorkerClient
    {
        
        public void SpawnObjectRequest(NetDataPackage dataPackage)
        {
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
            int destroyObjectServerID = dataPackage.GetInt();
            
            var destroyNetworkObjectEvent = new DestroyNetworkObjectEvent()
            {
                ServerID = destroyObjectServerID,
            };
            
            World.Default.CreateTickEvent().AddComponentData(destroyNetworkObjectEvent);
        }
        
        [NetworkCallback]
        private void DestroyObjectsList(NetPeer peer, NetDataPackage dataPackage)
        {
            var indestructibleObjectIds = dataPackage.GetIntArray();
        
            var clientMassDestroyEvent = new DestroyObjectsListEvent
            {
                DestructibleObjectIds = indestructibleObjectIds
            };
        
            World.Default.CreateTickEvent().AddComponentData(clientMassDestroyEvent);
        }
    }
}