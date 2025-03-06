using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [RequireComponent(typeof(RemoteObjectInterpolation))]
    public sealed class RemoteObjectInterpolationProvider : MonoProvider<RemoteObjectInterpolationComponent>
    {
        public void Awake()
        {
            ref RemoteObjectInterpolationComponent data = ref GetData();
            data.RemoteObjectInterpolation = GetComponent<RemoteObjectInterpolation>();
        }
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