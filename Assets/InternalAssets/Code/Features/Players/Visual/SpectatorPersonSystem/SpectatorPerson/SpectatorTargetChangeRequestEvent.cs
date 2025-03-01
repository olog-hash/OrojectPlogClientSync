using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpectatorTargetChangeRequestEvent : IComponent
    {
        public Entity SpectatorTarget;
    }
}