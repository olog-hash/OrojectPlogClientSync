using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DeadMarkerProvider : MonoProvider<DeadMarker>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DeadMarker : IComponent
    {

    }
}