using ProjectOlog.Code.Mechanics.Impact.Victims;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Replenish.Health
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ReplenishHealthRequestSystem : TickrateSystem
    {
        private Filter _replenishHealthRequestsFilter;
        private EntityReplenishHelper _entityReplenishHelper;
        
        public override void OnAwake()
        {
            _entityReplenishHelper = new EntityReplenishHelper();
            _replenishHealthRequestsFilter = World.Filter.With<ReplenishHealthEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _replenishHealthRequestsFilter)
            {
                ref var replenishHealthEvent = ref entityEvent.GetComponent<ReplenishHealthEvent>();
                ref var victimEntityEvent = ref entityEvent.GetComponent<EntityVictimEvent>();
                
                // Добавляем здоровья сущности в упрощенном виде
                _entityReplenishHelper.TryReplenishHealth(victimEntityEvent.VictimEntity,
                    replenishHealthEvent.ReplenishCount, out var _);
            }
        }
    }
}