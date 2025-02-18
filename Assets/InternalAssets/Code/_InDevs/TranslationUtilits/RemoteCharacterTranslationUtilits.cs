using ProjectOlog.Code._InDevs.Players.RemoteSync;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.TranslationUtilits
{
    /// <summary>
    /// Отвечает за работу с трансформом для удаленных Character обьектов (не локальных - серверных).
    /// </summary>
    public static class RemoteCharacterTranslationUtilits
    {
        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<MirrorInterpolationComponent>();
            
            translation.position = position;
            mirrorInterpolationComponent.MirrorInterpolation.SetPositionAndRotationClear(position, rotation);
        }
    }
}