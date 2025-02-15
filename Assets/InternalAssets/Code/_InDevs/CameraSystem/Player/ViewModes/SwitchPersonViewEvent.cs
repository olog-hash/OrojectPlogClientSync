using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.CameraSystem.Player.ViewModes
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SwitchPersonViewEvent : IComponent
    {
        public EPersonViewType ViewType;
    }
}