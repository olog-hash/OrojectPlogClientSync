using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.Rules
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DeathCameraTransitionRule : IComponent
    {
        [Header("MaxDelayDuration")]
        public static float MaxDelayDuration = 3;
        
        [Header("Parameters")]
        public bool IsTriggered;        
        public float DelayTimer;         
    }
}