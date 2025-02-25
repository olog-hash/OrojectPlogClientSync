using System;
using ProjectOlog.Code._InDevs.Players.RemoteSync;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Entities.Objects.Snapshot
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoteObjectInterpolationProvider : MonoProvider<RemoteObjectInterpolationComponent>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RemoteObjectInterpolationComponent: IComponent
    {
        public RemoteObjectInterpolation RemoteObjectInterpolation;
    }
}