using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Players.Interpolation;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.TranslationUtilits
{
    /// <summary>
    /// Отвечает за работу с трансформом для удаленных игроков (не локальных - серверных).
    /// </summary>
    public static class RemotePlayerTranslationUtilits
    {
        private static bool IsEntityAvaliable(Entity entity)
        {
            if (entity == null || entity.IsNullOrDisposed()) return false;
            if (!entity.Has<Translation>() || !entity.Has<RemotePlayerInterpolationComponent>()) return false;

            return true;
        }
        
        public static void SetPosition(Entity entity, Vector3 position)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemotePlayerInterpolationComponent>();
            
            translation.position = position;
            
            mirrorInterpolationComponent.RemotePlayerInterpolation.SetPositionAndRotationClear(position, translation.rotation);
        }
        
        public static void SetRotation(Entity entity, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemotePlayerInterpolationComponent>();
            
            translation.rotation = rotation;
            
            mirrorInterpolationComponent.RemotePlayerInterpolation.SetPositionAndRotationClear(translation.position, rotation);
        }
        
        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemotePlayerInterpolationComponent>();
            
            translation.position = position;
            translation.rotation = rotation;
            
            mirrorInterpolationComponent.RemotePlayerInterpolation.SetPositionAndRotationClear(position, rotation);
        }
    }
}