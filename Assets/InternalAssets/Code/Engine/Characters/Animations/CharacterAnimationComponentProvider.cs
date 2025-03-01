using System;
using Animancer;
using ProjectOlog.Code.Engine.Characters.Animations.Controllers;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.Animations
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterAnimationComponentProvider : MonoProvider<CharacterAnimationComponent>
    {
        private void Awake()
        {
            ref CharacterAnimationComponent data = ref GetData();
            data.LegAnimationController.Awake();
            data.BackAnimationController.Awake();
            data.WeaponAnimationController.Awake();
        }
    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterAnimationComponent : IComponent
    {
        public AnimancerComponent Animancer;
        
        [Header("Controllers")]
        public LegAnimationController LegAnimationController;
        public BackAnimationController BackAnimationController;
        public WeaponAnimationController WeaponAnimationController;
    }
}