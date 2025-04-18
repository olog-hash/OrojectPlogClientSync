﻿using System;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TransitionStateRequestComponent : IComponent
    {
        public bool IsActive;
        public CharacterState NextCharacterState;
    }
}