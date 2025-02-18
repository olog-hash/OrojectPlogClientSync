using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.TranslationUtilits
{
    /// <summary>
    /// Отвечает за работу с трансформом для Character обьекта (как правило это локальный игрок).
    /// </summary>
    public static class CharacterTranslationUtilits
    {
        public static void SetPosition(Entity entity, Vector3 position)
        {
            ref var translation  = ref entity.GetComponent<Translation>();
            ref var characterInterpolation = ref entity.GetComponent<CharacterInterpolation>();
            
            translation.position = position;
            characterInterpolation.CurrentTransform.position = position;
            characterInterpolation.PreviousTransform.position = position;
            
            characterInterpolation.SkipNextInterpolation();
        }

        public static void SetRotation(Entity entity, Quaternion rotation)
        {
            ref var translation  = ref entity.GetComponent<Translation>();
            ref var characterInterpolation = ref entity.GetComponent<CharacterInterpolation>();
            
            translation.rotation = rotation;
            characterInterpolation.CurrentTransform.rotation = rotation;
            characterInterpolation.PreviousTransform.rotation = rotation;
            
            characterInterpolation.SkipNextInterpolation();
        }

        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            ref var translation  = ref entity.GetComponent<Translation>();
            ref var characterInterpolation = ref entity.GetComponent<CharacterInterpolation>();
            
            translation.rotation = rotation;
            characterInterpolation.CurrentTransform.rotation = rotation;
            characterInterpolation.PreviousTransform.rotation = rotation;
            
            translation.position = position;
            characterInterpolation.CurrentTransform.position = position;
            characterInterpolation.PreviousTransform.position = position;
            
            characterInterpolation.SkipNextInterpolation();
        }
    }
}