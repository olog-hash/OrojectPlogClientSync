using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Replenish.Events;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Repercussion.Damage
{
    public sealed class BasicHealthNetworker : NetWorkerClient
    {
        [NetworkCallback]
        public void ReplenishHealth(NetPeer peer, NetDataPackage dataPackage)
        {
            int serverID = dataPackage.GetInt();
            int replenishCount = dataPackage.GetInt();

            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var networkEntity)) return;

            var replenishHealthEvent = new ReplenishHealthEvent
            {
                VictimEntity = networkEntity.Entity,
                ReplenishCount = replenishCount,
            };
            
            World.Default.CreateTickEvent().AddComponentData(replenishHealthEvent);
        }
        
        [NetworkCallback]
        public void ReplenishArmor(NetPeer peer, NetDataPackage dataPackage)
        {
            int serverID = dataPackage.GetInt();
            int replenishCount = dataPackage.GetInt();
            
            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var networkEntity)) return;
            
            var replenishArmorEvent = new ReplenishArmorEvent
            {
                VictimEntity = networkEntity.Entity,
                ReplenishCount = replenishCount,
            };
            
            World.Default.CreateTickEvent().AddComponentData(replenishArmorEvent);
        }
    }
}