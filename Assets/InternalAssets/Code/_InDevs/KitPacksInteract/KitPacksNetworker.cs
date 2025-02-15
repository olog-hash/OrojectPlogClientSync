using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Infrastructure;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code._InDevs.KitPacksInteract
{
    public sealed class KitPacksNetworker : NetWorkerClient
    {
        [NetworkCallback]
        private void KitPackInteract(NetPeer peer, NetDataPackage dataPackage)
        {
            int victimID = dataPackage.GetInt();
            int kitPackID = dataPackage.GetInt();

            if (!_entitiesContainer.TryGetNetworkEntity(victimID, out var victim)) return;
            if (!_entitiesContainer.TryGetNetworkEntity(kitPackID, out var kitPack)) return;

            var kitPackInteractEvent = new KitPackInteractEvent
            {
                VictimEntity = victim.Entity,
                KitPackEntity = kitPack.Entity
            };

            World.Default.CreateTickEvent().AddComponentData(kitPackInteractEvent);
        }
    }
}