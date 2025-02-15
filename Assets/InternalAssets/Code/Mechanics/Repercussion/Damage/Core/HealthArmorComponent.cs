using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Mechanics.Repercussion.Damage.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthArmorComponentProvider : MonoProvider<HealthArmorComponent>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct HealthArmorComponent : IComponent
    {
        [Header("CurrentData")]
        public int MaxHealth;
        public int MaxArmor;
        
        [Header("Target values")]
        public int Health;
        public int Armor;

        public int HealthPointLeft => Health + Armor;
    }
}