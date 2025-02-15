using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Entities.Objects.Interactables.Core.Events;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Objects
{
    public sealed class InteractiveObjectNetworker : NetWorkerClient
    {
        public void UseInteractionObjectRequest(NetDataPackage dataPackage)
        {
            SendTo(nameof(UseInteractionObjectRequest), dataPackage, DeliveryMethod.ReliableOrdered);
        }

        // Отправка данных по внутреннему нетворкеру обьекта.
        [NetworkCallback]
        private void UpdatingInternalNetworker(NetPeer peer, NetDataPackage dataPackage)
        {
            int serverID = dataPackage.GetInt();
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
            int serverID = dataPackage.GetInt();
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