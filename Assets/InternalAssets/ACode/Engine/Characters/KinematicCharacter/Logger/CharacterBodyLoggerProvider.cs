using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterBodyLoggerProvider : MonoProvider<CharacterBodyLogger>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterBodyLogger : IComponent
    {
        public float ViewPitchDegrees;
        public float PreviousFallVelocity;
        public bool IsGrounded;
        public Vector2 MoveDirection;
        public ECharacterBodyState CharacterBodyState;
    }
}