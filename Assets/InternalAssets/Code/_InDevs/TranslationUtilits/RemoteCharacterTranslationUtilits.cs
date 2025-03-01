using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Players.Interpolation;
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
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemotePlayerInterpolationComponent>();
            
            translation.position = position;
            mirrorInterpolationComponent.RemotePlayerInterpolation.SetPositionAndRotationClear(position, rotation);
        }
    }
}