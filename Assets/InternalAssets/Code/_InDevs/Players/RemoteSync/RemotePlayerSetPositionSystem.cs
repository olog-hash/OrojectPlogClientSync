using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.RemoteSync
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemotePlayerSetPositionSystem : TickrateSystem
    {
        private Filter _remotePlayersFilter;
        
        public override void OnAwake()
        {
            _remotePlayersFilter = World.Filter.With<SetPositionRotation>().With<Translation>()
                .With<MirrorInterpolationComponent>().Without<LocalPlayerMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _remotePlayersFilter)
            {
                ref var translation = ref entity.GetComponent<Translation>();
                ref var setPositionComponent = ref entity.GetComponent<SetPositionRotation>();
                ref var mirrorInterpolationComponent = ref entity.GetComponent<MirrorInterpolationComponent>();
                
                //mirrorInterpolationComponent.MirrorInterpolation.SetPositionAndRotation(setPositionComponent.Position, setPositionComponent.Rotation);

                entity.RemoveComponent<SetPositionRotation>();
            }
        }
    }
}