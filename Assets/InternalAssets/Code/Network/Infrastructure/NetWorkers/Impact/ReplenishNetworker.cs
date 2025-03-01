using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Unpackers.Impact;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Impact
{
    public sealed class ReplenishNetworker : NetWorkerClient
    {
        private ReplenishEventsUnpacker _replenishEventsUnpacker;

        public ReplenishNetworker(ReplenishEventsUnpacker replenishEventsUnpacker)
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