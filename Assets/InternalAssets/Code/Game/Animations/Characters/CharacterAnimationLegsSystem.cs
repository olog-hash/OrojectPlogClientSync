﻿using ProjectOlog.Code.Game.Animations.Characters.Core;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Game.Animations.Characters
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterAnimationLegsSystem : UpdateSystem
    {
        private Filter _filter;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<CharacterBodyLogger>().With<CharacterAnimationComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var characterAnimationComponent = ref entity.GetComponent<CharacterAnimationComponent>();
                var characterBodyLogger = entity.GetComponent<CharacterBodyLogger>();

                UpdateController(characterAnimationComponent.LegAnimationController, characterBodyLogger);
                UpdateController(characterAnimationComponent.BackAnimationController, characterBodyLogger);
                //UpdateController(characterAnimationComponent.WeaponAnimationController, characterBodyLogger);
            }
        }

        private void UpdateController(IAnimationController animationController, CharacterBodyLogger characterBodyLogger)
        {
            animationController.UpdateAnimation(characterBodyLogger);
        }
    }
}