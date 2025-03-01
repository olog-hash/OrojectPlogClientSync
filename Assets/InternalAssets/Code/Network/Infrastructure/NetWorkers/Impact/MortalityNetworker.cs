using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Unpackers.Impact;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Mortality;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Impact
{
    public sealed class MortalityNetworker : NetWorkerClient
    {
        private MortalityEventsUnpacker _mortalityEventsUnpacker;

        public MortalityNetworker(MortalityEventsUnpacker mortalityEventsUnpacker)
        {
            _mortalityEventsUnpacker = mortalityEventsUnpacker;
        }
        
        [NetworkCallback]
        private void DamageCompoundEvent(NetPeer peer, NetDataPackage dataPackage)
        {
            var damagePacket = new DamageEventPacket();
            damagePacket.Deserialize(dataPackage);
            
            _mortalityEventsUnpacker.UnpackDamageEventPacket(damagePacket);
        }
        
        [NetworkCallback]
        private void DeathCompoundEvent(NetPeer peer, NetDataPackage dataPackage)
        {
            var deathPacket = new DeathEventPacket();
            deathPacket.Deserialize(dataPackage);
            
            _mortalityEventsUnpacker.UnpackDeathEventPacket(deathPacket);
        }
    }
}