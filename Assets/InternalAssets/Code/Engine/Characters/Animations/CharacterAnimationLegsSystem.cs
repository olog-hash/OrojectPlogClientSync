using ProjectOlog.Code.Engine.Characters.Animations.Core;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Engine.Characters.Animations
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterAnimationLegsSystem : UpdateSystem
    {
        private Filter _filter;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<Translation>().With<CharacterBodyLogger>().With<CharacterAnimationComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var translation = ref entity.GetComponent<Translation>();
                ref var characterAnimationComponent = ref entity.GetComponent<CharacterAnimationComponent>();
                var characterBodyLogger = entity.GetComponent<CharacterBodyLogger>();

                // Обновляем приоритетные положения
                characterAnimationComponent.LegAnimationController.UpdateMovement(translation.position,
                    translation.rotation);
                
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