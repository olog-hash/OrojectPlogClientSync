using System;
using ProjectOlog.Code.Engine.Cameras.ViewControllers.Markers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Entities.ShieldProtect
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ShieldProtectComponentProvider : MonoProvider<ShieldProtectComponent>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ShieldProtectComponent : IComponent
    {
        public bool IsActive;
        public float ShieldTime;
        public BaseObjectViewMarker ShieldObject;
    }
}