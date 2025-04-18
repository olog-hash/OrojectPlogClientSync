﻿using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Gameplay.ECS.Rules.GameRules.FirstAppeared.Spawn
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnRequestRule : IComponent
    {
        public const float MaxCooldown = 5;
        public float Cooldown;
    }
}