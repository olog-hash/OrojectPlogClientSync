using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Infrastructure.CompoundEvents.Unpackers;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets.Mortality;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Repercussion.Damage
{
    public sealed class BasicMortalityNetworker : NetWorkerClient
    {
        private MortalityEventsUnpacker _mortalityEventsUnpacker;

        public BasicMortalityNetworker(MortalityEventsUnpacker mortalityEventsUnpacker)
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