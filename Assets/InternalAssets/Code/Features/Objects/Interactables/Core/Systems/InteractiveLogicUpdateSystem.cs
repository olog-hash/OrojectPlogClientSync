using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Objects.Interactables.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InteractiveLogicUpdateSystem : TickrateSystem
    {
        private Filter _interactionObjectFilter;
        
        public override void OnAwake()
        {
            _interactionObjectFilter = World.Filter.With<InteractionObjectComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _interactionObjectFilter)
            {
                ref var interactionObjectComponent = ref entity.GetComponent<InteractionObjectComponent>();
                
                interactionObjectComponent.ObjectStateManager.OnLogicUpdate(deltaTime);
            }
        }
    }
}