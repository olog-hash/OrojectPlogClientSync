using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.DeathCameraTransitionRule
{
    /// <summary>
    /// Компонент-правило, определяющий правило перехода камеры после смерти игрока.
    /// Контролирует задержку перед автоматическим переключением камеры на другого игрока.
    /// </summary>
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DeathCameraTransitionRule : IComponent
    {
        /// <summary>
        /// Максимальная длительность задержки перед переключением камеры в секундах.
        /// </summary>
        public static float MaxDelayDuration = 3;
        
        /// <summary>
        /// Указывает, активировано ли правило перехода камеры.
        /// </summary>
        public bool IsActive;
        
        /// <summary>
        /// Оставшееся время задержки перед переключением камеры в секундах.
        /// </summary>
        public float RemainingDelay;         
    }
}