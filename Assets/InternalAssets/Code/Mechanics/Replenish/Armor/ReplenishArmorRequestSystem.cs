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
    public sealed class ReplenishArmorRequestSystem : TickrateSystem
    {
        private Filter _replenishArmorRequestsFilter;
        private EntityReplenishHelper _entityReplenishHelper;
        
        public override void OnAwake()
        {
            _entityReplenishHelper = new EntityReplenishHelper();
            _replenishArmorRequestsFilter = World.Filter.With<ReplenishArmorEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _replenishArmorRequestsFilter)
            {
                ref var replenishArmorEvent = ref entityEvent.GetComponent<ReplenishArmorEvent>();

                ReplenishArmor(replenishArmorEvent);
            }
        }

        private void ReplenishArmor(ReplenishArmorEvent replenishArmorEvent)
        {
            _entityReplenishHelper.TryReplenishArmor(replenishArmorEvent.VictimEntity,
                replenishArmorEvent.ReplenishCount, out int _);
        }
    }
}