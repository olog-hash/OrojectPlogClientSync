using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemotePlayerInterpolationSystem : UpdateSystem  // Изменить на UpdateSystem вместо TickrateSystem
    {
        private Filter _remotePlayerFilter;

        public override void OnAwake()
        {
            _remotePlayerFilter = World.Filter
                .With<Translation>()
                .With<RemotePlayerInterpolationComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _remotePlayerFilter)
            {
                ref var mirrorInterpolation = ref entity.GetComponent<RemotePlayerInterpolationComponent>();
                ref var characterBodyLogger = ref entity.GetComponent<CharacterBodyLogger>();
        
                // Обновление позиции с использованием сглаживателя
                mirrorInterpolation.RemotePlayerInterpolation.UpdatePosition(deltaTime, ref characterBodyLogger);
            }
        }
    }
}