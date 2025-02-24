using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Mortality.Damage
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PostDamageEvent : IComponent
    {
        public int ActualDamageCount;
    }
}