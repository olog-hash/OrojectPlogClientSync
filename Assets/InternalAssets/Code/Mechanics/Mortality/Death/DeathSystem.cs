using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Mortality.Death
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DeathSystem : TickrateSystem
    {
        private Filter _deathFilter;
        private EntityMortalityHelper _entityMortalityHelper;
        
        public override void OnAwake()
        {
            _entityMortalityHelper = new EntityMortalityHelper();
            
            _deathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _deathFilter)
            {
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();

                // Убиваем сущность в упрощенном виде
                _entityMortalityHelper.TryKill(entityVictimEvent.VictimEntity);
            }
        }
    }
}