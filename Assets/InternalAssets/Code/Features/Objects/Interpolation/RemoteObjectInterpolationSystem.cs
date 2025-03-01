using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Objects.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoteObjectInterpolationSystem : UpdateSystem  // Изменить на UpdateSystem вместо TickrateSystem
    {
        private Filter _remoteObjectsFilter;

        public override void OnAwake()
        {
            _remoteObjectsFilter = World.Filter
                .With<Translation>()
                .With<RemoteObjectInterpolationComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _remoteObjectsFilter)
            {
                ref var mirrorInterpolation = ref entity.GetComponent<RemoteObjectInterpolationComponent>();
            
                // Обновление позиции должно происходить в Update цикле
                mirrorInterpolation.RemoteObjectInterpolation.UpdatePosition(deltaTime);
            }
        }
    }
}