using ProjectOlog.Code.Features.Objects.Interactables.Core.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Objects.Interactables.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoteStateTransitionBroadcastSystem : TickrateSystem
    {
        private Filter _stateTransitionObjectEvents;

        public override void OnAwake()
        {
            _stateTransitionObjectEvents = World.Filter.With<RemoteObjectStateTransitionEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _stateTransitionObjectEvents)
            {
                ref var remoteStateTransitionEvent = ref entityEvent.GetComponent<RemoteObjectStateTransitionEvent>();
                
                ObjectStateTransition(remoteStateTransitionEvent);
                
                entityEvent.Dispose();
            }
        }

        // Синхронизируем плавный переход между состояний исходя от инфы с сервера
        private void ObjectStateTransition(RemoteObjectStateTransitionEvent remoteStateTransitionEvent)
        {
            if (remoteStateTransitionEvent.ObjectProvider is null) return;
            
            var networkObject = remoteStateTransitionEvent.ObjectProvider;
            
            if (networkObject != null && networkObject.Has<InteractionObjectComponent>())
            {
                var interactionObjectComponent = networkObject.Entity.GetComponent<InteractionObjectComponent>();
                var objectStateManager = interactionObjectComponent.ObjectStateManager;
                
                // Устанавливаем текущее состояние через плавный переход
                var currentState = objectStateManager.GetEnumFromInt(remoteStateTransitionEvent.CurrentStateKey);
                objectStateManager.TransitionState(currentState);
            }
        }
    }
}