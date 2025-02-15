using ProjectOlog.Code.Entities.Objects.Interactables.Core.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Entities.Objects.Interactables.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoteDataBroadcastSystem : TickrateSystem
    {
        private Filter _dataBroadcastEvents;

        public override void OnAwake()
        {
            _dataBroadcastEvents = World.Filter.With<RemoteObjectDataBroadcast>().Build();
        }

        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _dataBroadcastEvents)
            {
                ref var remoteObjectDataEvent = ref entityEvent.GetComponent<RemoteObjectDataBroadcast>();

                RemoteDataBroadcast(remoteObjectDataEvent);
                
                entityEvent.Dispose();
            }
        }

        // Передаем все пакеты от сервера и прокидываем их в нетворкер к обьекту для синхронизации.
        private void RemoteDataBroadcast(RemoteObjectDataBroadcast remoteObjectDataEvent)
        {
            if (remoteObjectDataEvent.DataPackagesArray == null ||
                remoteObjectDataEvent.DataPackagesArray.Length == 0) return;

            if (remoteObjectDataEvent.ObjectProvider.IsNullOrDisposed()) return;

            var networkObject = remoteObjectDataEvent.ObjectProvider;

            if (networkObject != null && networkObject.Has<InteractionObjectComponent>())
            {
                var interactionObjectComponent = networkObject.Entity.GetComponent<InteractionObjectComponent>();
                var objectStateManager = interactionObjectComponent.ObjectStateManager;

                for (int i = 0; i < remoteObjectDataEvent.DataPackagesArray.Length; i++)
                {
                    objectStateManager.ObjectNetworkerContainer.HandleRequest(
                        remoteObjectDataEvent.DataPackagesArray[i]);
                }
            }
        }
    }
}