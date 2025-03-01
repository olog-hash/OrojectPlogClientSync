using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Features.Objects.Interactables.Core.Events;
using ProjectOlog.Code.Network.Infrastructure.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Entities
{
    public sealed class InteractiveObjectNetworker : NetWorkerClient
    {
        public void UseInteractionObjectRequest(ushort serverID)
        {
            SendTo(nameof(UseInteractionObjectRequest), new NetDataPackage(serverID), DeliveryMethod.ReliableOrdered);
        }

        // Отправка данных по внутреннему нетворкеру обьекта.
        [NetworkCallback]
        private void UpdatingInternalNetworker(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort serverID = dataPackage.GetUShort();
            var dataPacketsArray = dataPackage.GetPackagesArray();

            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var objectProvider)) return;
            
            var remoteObjectDataBroadcast = new RemoteObjectDataBroadcast
            {
                ObjectProvider = objectProvider,
                DataPackagesArray = dataPacketsArray
            };
            
            World.Default.CreateTickEvent().AddComponentData(remoteObjectDataBroadcast);
        }

        // Оповещаем об изменении состояния обьекта
        [NetworkCallback]
        private void TransitionObjectState(NetPeer peer, NetDataPackage dataPackage)
        {
            ushort serverID = dataPackage.GetUShort();
            short currentStateKey = dataPackage.GetShort();

            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var objectProvider)) return;
            
            var remoteObjectStateTransition = new RemoteObjectStateTransitionEvent
            {
                ObjectProvider = objectProvider,
                CurrentStateKey =  currentStateKey,
            };
            
            World.Default.CreateTickEvent().AddComponentData(remoteObjectStateTransition);
        }
    }
}