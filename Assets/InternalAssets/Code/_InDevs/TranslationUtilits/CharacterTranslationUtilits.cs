using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Engine.Transform;
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
            characterInterpolation.CurrentTransform.Position = position;
            characterInterpolation.PreviousTransform.Position = position;
            
            characterInterpolation.SkipNextInterpolation();
        }

        public static void SetRotation(Entity entity, Quaternion rotation)
        {
            ref var translation  = ref entity.GetComponent<Translation>();
            ref var characterInterpolation = ref entity.GetComponent<CharacterInterpolation>();
            
            translation.rotation = rotation;
            characterInterpolation.CurrentTransform.Rotation = rotation;
            characterInterpolation.PreviousTransform.Rotation = rotation;
            
            characterInterpolation.SkipNextInterpolation();
        }

        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            ref var translation  = ref entity.GetComponent<Translation>();
            ref var characterInterpolation = ref entity.GetComponent<CharacterInterpolation>();
            
            translation.rotation = rotation;
            characterInterpolation.CurrentTransform.Rotation = rotation;
            characterInterpolation.PreviousTransform.Rotation = rotation;
            
            translation.position = position;
            characterInterpolation.CurrentTransform.Position = position;
            characterInterpolation.PreviousTransform.Position = position;
            
            characterInterpolation.SkipNextInterpolation();
        }
    }
}