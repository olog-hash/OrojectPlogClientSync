using ProjectOlog.Code.Mechanics.Impact.Victims;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Mortality.Damage
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PostDamageSystem : TickrateSystem
    {
        private Filter _postDamageFilter;
        private EntityMortalityHelper _entityMortalityHelper;
        
        public override void OnAwake()
        {
            _entityMortalityHelper = new EntityMortalityHelper();
            
            _postDamageFilter = World.Filter.With<PostDamageEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _postDamageFilter)
            {
                ref var postDamageEvent = ref entityEvent.GetComponent<PostDamageEvent>();
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();

                // Наносим дамаг
                _entityMortalityHelper.TryMakeDamage(entityVictimEvent.VictimEntity, postDamageEvent.ActualDamageCount);
            }
        }
    }
}