using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.DamageRequest
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PostDamageSystem : TickrateSystem
    {
        private Filter _playerDeathFilter;
        private EntityLifeProcessor _entityLifeProcessor;

        public override void OnAwake()
        {
            _entityLifeProcessor = new EntityLifeProcessor();
            
            _playerDeathFilter = World.Filter.With<PostDamageEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                DamageRequestEvent(entityEvent);
            }
        }

        private void DamageRequestEvent(Entity entity)
        {
            ref var postDamageEvent = ref entity.GetComponent<PostDamageEvent>();

            _entityLifeProcessor.DamageEntity(postDamageEvent.VictimEntity, postDamageEvent.RealDamageCount, out _);
        }
    }
}