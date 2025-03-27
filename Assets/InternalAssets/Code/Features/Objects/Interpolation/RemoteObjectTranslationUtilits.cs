using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Objects.Interpolation;
using ProjectOlog.Code.Features.Players.Interpolation;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.TranslationUtilits
{
    /// <summary>
    /// Отвечает за работу с трансформом для удаленных обьектов (не локальных - серверных).
    /// </summary>
    public static class RemoteObjectTranslationUtilits
    {
        private static bool IsEntityAvaliable(Entity entity)
        {
            if (entity == null || entity.IsNullOrDisposed()) return false;
            if (!entity.Has<Translation>() || !entity.Has<RemoteObjectInterpolationComponent>()) return false;

            return true;
        }
        
        public static void SetPosition(Entity entity, Vector3 position)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemoteObjectInterpolationComponent>();
            
            translation.position = position;
            
            mirrorInterpolationComponent.RemoteObjectInterpolation.SetPositionAndRotationClear(position, translation.rotation);
        }
        
        public static void SetRotation(Entity entity, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemoteObjectInterpolationComponent>();
            
            translation.rotation = rotation;
            
            mirrorInterpolationComponent.RemoteObjectInterpolation.SetPositionAndRotationClear(translation.position, rotation);
        }
        
        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;
            
            ref var translation = ref entity.GetComponent<Translation>();
            ref var mirrorInterpolationComponent = ref entity.GetComponent<RemoteObjectInterpolationComponent>();
            
            translation.position = position;
            translation.rotation = rotation;
            
            mirrorInterpolationComponent.RemoteObjectInterpolation.SetPositionAndRotationClear(position, rotation);
        }
    }
}