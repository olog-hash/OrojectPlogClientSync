using System;
using ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews.Markers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer
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