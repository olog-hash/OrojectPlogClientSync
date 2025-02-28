using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer.Events;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer
{
    public sealed class ShieldProtectNetworker : NetWorkerClient
    {
        [NetworkCallback]
        private void ShieldAdded(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort serverID = dataPackage.GetUShort();
            float shieldTime = dataPackage.GetFloat();

            var shieldAddEvent = new ShieldAddedEvent
            {
                ServerID = serverID,
                ShieldTime = shieldTime
            };
            
            World.Default.CreateTickEvent().AddComponentData(shieldAddEvent);
        }

        [NetworkCallback]
        private void ShieldRemoved(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort serverID = dataPackage.GetUShort();

            var shieldRemoveEvent = new ShieldRemovedEvent
            {
                ServerID = serverID
            };
            
            World.Default.CreateTickEvent().AddComponentData(shieldRemoveEvent);
        }
    }
}