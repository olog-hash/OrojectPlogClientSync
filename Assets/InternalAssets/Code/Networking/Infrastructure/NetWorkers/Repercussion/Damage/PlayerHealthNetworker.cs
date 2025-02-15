using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Pressures;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.DamageRequest;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Handlers.ComponentHandlers;
using ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Deserializators;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets.Repercussion;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Repercussion.Damage
{
    public class PlayerHealthNetworker : NetWorkerClient
    {
        private ComponentSerializator _componentSerializator;
        private PressureDeserializer _pressureDeserializer;

        public PlayerHealthNetworker(ComponentSerializator componentSerializator)
        {
            _componentSerializator = componentSerializator;
            _pressureDeserializer = new PressureDeserializer(_componentSerializator);
        }

        // Сообщает информацию про дамаг
        [NetworkCallback]
        private void DamagePlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            var repercussionPacket = new RepercussionPacket();
            repercussionPacket.Deserialize(dataPackage);

            var tickEvent = CreateTickEventWithPlayerVictimMarker();

            TryDeserializeComponents<PostDamageEvent>(repercussionPacket, tickEvent);
        }

        // Уведомление об убийстве игрока для всех
        [NetworkCallback]
        private void KillPlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            var repercussionPacket = new RepercussionPacket();
            repercussionPacket.Deserialize(dataPackage);

            var tickEvent = CreateTickEventWithPlayerVictimMarker();

            TryDeserializeComponents<DeathEvent>(repercussionPacket, tickEvent);
        }
        
        private bool TryDeserializeComponents<TDetail>(RepercussionPacket repercussionPacket, Entity tickEvent)
            where TDetail : struct, IComponent
        {
            _componentSerializator.TryDeserializeToEntity<TDetail>(repercussionPacket.DetailPacket, tickEvent);
            _pressureDeserializer.TryDeserializeToEntity(repercussionPacket.PressureType, repercussionPacket.PressurePacket, tickEvent);

            return tickEvent.Has<PlayerVictimMarker>() && tickEvent.Has<TDetail>();
        }
        
        private Entity CreateTickEventWithPlayerVictimMarker()
        {
            var tickEvent = World.Default.CreateTickEvent();
            tickEvent.AddComponent<PlayerVictimMarker>();
            return tickEvent;
        }
    }
}