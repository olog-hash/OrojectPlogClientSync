using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core;
using ProjectOlog.Code.Mechanics.Replenish.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Replenish
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ReplenishHealthRequestSystem : TickrateSystem
    {
        private Filter _replenishHealthRequestsFilter;
        private EntityLifeProcessor _entityLifeProcessor;
        
        public override void OnAwake()
        {
            _entityLifeProcessor = new EntityLifeProcessor();
            _replenishHealthRequestsFilter = World.Filter.With<ReplenishHealthEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _replenishHealthRequestsFilter)
            {
                ref var replenishHealthEvent = ref entityEvent.GetComponent<ReplenishHealthEvent>();

                ReplenishArmor(replenishHealthEvent);
            }
        }

        private void ReplenishArmor(ReplenishHealthEvent replenishHealthEvent)
        {
            _entityLifeProcessor.ReplenishHealth(replenishHealthEvent.VictimEntity,
                replenishHealthEvent.ReplenishCount, out var _);
        }
    }
}