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
    public class BasicMortalityNetworker : NetWorkerClient
    {
        private MortalityEventUnpacker _mortalityEventUnpacker;

        public BasicMortalityNetworker(MortalityEventUnpacker mortalityEventUnpacker)
        {
            _mortalityEventUnpacker = mortalityEventUnpacker;
        }

        // Сообщает информацию про дамаг
        [NetworkCallback]
        private void DamageCompoundEvent(NetPeer peer, NetDataPackage dataPackage)
        {
            var damagePacket = new DamageEventPacket();
            damagePacket.Deserialize(dataPackage);
            
            _mortalityEventUnpacker.UnpackDamageEventPacket(damagePacket);
        }

        // Уведомление об убийстве игрока для всех
        [NetworkCallback]
        private void DeathCompoundEvent(NetPeer peer, NetDataPackage dataPackage)
        {
            var deathPacket = new DeathEventPacket();
            deathPacket.Deserialize(dataPackage);
            
            _mortalityEventUnpacker.UnpackDeathEventPacket(deathPacket);
        }
    }
}