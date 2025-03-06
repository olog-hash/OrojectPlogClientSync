using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [RequireComponent(typeof(RemotePlayerInterpolation))]
    public sealed class RemotePlayerInterpolationProvider : MonoProvider<RemotePlayerInterpolationComponent>
    {
        public void Awake()
        {
            ref RemotePlayerInterpolationComponent data = ref GetData();
            data.RemotePlayerInterpolation = GetComponent<RemotePlayerInterpolation>();
        }
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RemotePlayerInterpolationComponent : IComponent
    {
        public RemotePlayerInterpolation RemotePlayerInterpolation;
    }
}