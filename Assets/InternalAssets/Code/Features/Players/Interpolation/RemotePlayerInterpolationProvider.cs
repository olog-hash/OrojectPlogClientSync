using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemotePlayerInterpolationProvider : MonoProvider<RemotePlayerInterpolationComponent>
    {
        private void Awake()
        {
            ref RemotePlayerInterpolationComponent data = ref GetData();
            data.MovementSmoother = new CharacterMovementSmoother();
        }
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RemotePlayerInterpolationComponent : IComponent
    {
        public RemotePlayerInterpolation RemotePlayerInterpolation;
        public CharacterMovementSmoother MovementSmoother;
    }
}