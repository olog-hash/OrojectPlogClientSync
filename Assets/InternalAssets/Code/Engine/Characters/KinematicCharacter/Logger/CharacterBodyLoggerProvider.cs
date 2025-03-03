using System;
using ProjectOlog.Code.Engine.Characters.Animations.Controllers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger
{
    /// <summary>
    /// Провайдер для компонента логирования состояния тела персонажа
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterBodyLoggerProvider : MonoProvider<CharacterBodyLogger>
    {

    }

    /// <summary>
    /// Компонент для отслеживания и хранения текущего состояния тела персонажа
    /// </summary>  
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
        public DetailedMovementDirection MovementDirection;
        public ECharacterBodyState CharacterBodyState;
    }
}