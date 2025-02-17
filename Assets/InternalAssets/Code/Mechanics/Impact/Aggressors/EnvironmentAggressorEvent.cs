using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Impact.Aggressors
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EnvironmentAggressorEvent : IComponent
    {
        public EEnvironmentType EnvironmentType;
    }
    
    public enum EEnvironmentType
    {
        None,
        DamageZone,
        Void,
        FallHight,
    }
}