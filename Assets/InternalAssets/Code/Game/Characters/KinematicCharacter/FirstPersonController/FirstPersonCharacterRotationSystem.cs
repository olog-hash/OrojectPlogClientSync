using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FirstPersonCharacterRotationSystem : UpdateSystem 
    {
        private Filter filter;

        public override void OnAwake()
        {
            filter = World.Filter.With<FirstPersonCharacter>().With<CharacterInterpolation>().With<FirstPersonInputs>().Without<DeadMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in filter)
            {
                ref var character = ref GetComponent<FirstPersonCharacter>(entity);
                ref var characterInterpolation = ref GetComponent<CharacterInterpolation>(entity);
                ref var characterTranslation = ref GetComponent<Translation>(entity);
                ref var viewTranslation = ref GetComponent<Translation>(character.CharacterViewEntity);
                var characterInputs = GetComponent<FirstPersonInputs>(entity);

                Quaternion characterRotation = characterTranslation.rotation;
                Quaternion localViewRotation = viewTranslation.localRotation;

                // Compute character & view rotations from rotation input
                FirstPersonCharacterUtilities.ComputeFinalRotationsFromRotationDelta(
                    ref characterRotation,
                    ref character.ViewYawDegrees,
                    ref character.ViewPitchDegrees,
                    characterInputs.LookYawPitchDegrees,
                    0f,
                    character.MinVAngle,
                    character.MaxVAngle,
                    out float canceledPitchDegrees,
                    out localViewRotation);


                // Apply character & view rotations
                characterTranslation.rotation = characterRotation;
                viewTranslation.localRotation = localViewRotation;

                characterInterpolation.CurrentTransform.rotation = characterRotation;
            }
        }
    }
}

/*

foreach (var entity in filter)
            {
                ref var character = ref GetComponent<FirstPersonCharacter>(entity);
                ref var characterInterpolation = ref GetComponent<CharacterInterpolation>(entity);
                ref var characterTranslation = ref GetComponent<Translation>(entity);
                ref var viewTranslation = ref GetComponent<Translation>(character.CharacterViewEntity);
                var characterInputs = GetComponent<FirstPersonInputs>(entity);

                // Получаем текущую интерполированную позицию из системы интерполяции
                Vector3 interpolatedPosition = characterInterpolation.CurrentTransform.position;
            
                // Используем обычную логику поворота, но не трогаем позицию персонажа
                Quaternion characterRotation = characterTranslation.rotation;
                Quaternion localViewRotation = viewTranslation.localRotation;

                FirstPersonCharacterUtilities.ComputeFinalRotationsFromRotationDelta(
                    ref characterRotation,
                    ref character.ViewYawDegrees,
                    ref character.ViewPitchDegrees,
                    characterInputs.LookYawPitchDegrees,
                    0f,
                    character.MinVAngle,
                    character.MaxVAngle,
                    out float canceledPitchDegrees,
                    out localViewRotation);

                // Применяем повороты
                characterTranslation.rotation = characterRotation;
                viewTranslation.localRotation = localViewRotation;

                // Обновляем только rotation в интерполяции
                characterInterpolation.CurrentTransform.rotation = characterRotation;

                // Обновляем позицию камеры относительно интерполированной позиции персонажа
                Vector3 characterUp = characterRotation * Vector3.up;
                viewTranslation.localPosition = characterUp * character.CameraPointHeight;
            }
            */