using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.SpawnPlayerRequestRule
{
    /// <summary>
    /// Компонент-правило, определяющий правило запроса на возрождение игрока.
    /// Контролирует кулдаун между возможными возрождениями.
    /// </summary>
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnRequestRule : IComponent
    {
        /// <summary>
        /// Максимальное время кулдауна между возрождениями в секундах.
        /// </summary>
        public const float MaxCooldown = 1f;
        
        /// <summary>
        /// Оставшееся время кулдауна до возможности возрождения в секундах.
        /// </summary>
        public float RemainingCooldown;
    }
}