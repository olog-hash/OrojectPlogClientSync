using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Replenish.Events;
using ProjectOlog.Code.Networking.Infrastructure.CompoundEvents.Unpackers;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets.Impact.Replenish;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Repercussion.Damage
{
    public sealed class BasicReplenishNetworker : NetWorkerClient
    {
        private ReplenishEventsUnpacker _replenishEventsUnpacker;

        public BasicReplenishNetworker(ReplenishEventsUnpacker replenishEventsUnpacker)
        {
            _replenishEventsUnpacker = replenishEventsUnpacker;
        }

        [NetworkCallback]
        public void ReplenishHealth(NetPeer peer, NetDataPackage dataPackage)
        {
            var replenishHealthPacket = new ReplenishHealthEventPacket();
            replenishHealthPacket.Deserialize(dataPackage);
            
            _replenishEventsUnpacker.UnpackReplenishHealthEventPacket(replenishHealthPacket);
        }
        
        [NetworkCallback]
        public void ReplenishArmor(NetPeer peer, NetDataPackage dataPackage)
        {
            var replenishArmorPacket = new ReplenishArmorEventPacket();
            replenishArmorPacket.Deserialize(dataPackage);
            
            _replenishEventsUnpacker.UnpackReplenishArmorEventPacket(replenishArmorPacket);
        }
    }
}