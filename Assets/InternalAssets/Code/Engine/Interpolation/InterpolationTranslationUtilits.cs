using ProjectOlog.Code.Engine.Interpolation;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.TranslationUtilits
{
    /// <summary>
    /// Отвечает за работу с трансформом для локальной интерполяции.
    /// </summary>
    public static class InterpolationTranslationUtilits
    {
        private static bool IsEntityAvaliable(Entity entity)
        {
            if (entity == null || entity.IsNullOrDisposed()) return false;
            if (!entity.Has<Translation>() || !entity.Has<Interpolation>()) return false;

            return true;
        }
        
        public static void SetPosition(Entity entity, Vector3 position)
        {
            if (!IsEntityAvaliable(entity)) return;

            ref var translation = ref entity.GetComponent<Translation>();
            ref var interpolationComponent = ref entity.GetComponent<Interpolation>();
            
            translation.position = position;
            interpolationComponent.CurrentTransform.Position = position;
            interpolationComponent.PreviousTransform.Position = position;
            
            interpolationComponent.SkipNextInterpolation();
        }
        
        public static void SetRotation(Entity entity, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;

            ref var translation = ref entity.GetComponent<Translation>();
            ref var interpolationComponent = ref entity.GetComponent<Interpolation>();

            translation.rotation = rotation;
            interpolationComponent.CurrentTransform.Rotation = rotation;
            interpolationComponent.PreviousTransform.Rotation = rotation;
            
            interpolationComponent.SkipNextInterpolation();
        }
        
        public static void SetPositionAndRotation(Entity entity, Vector3 position, Quaternion rotation)
        {
            if (!IsEntityAvaliable(entity)) return;

            ref var translation = ref entity.GetComponent<Translation>();
            ref var interpolationComponent = ref entity.GetComponent<Interpolation>();

            translation.rotation = rotation;
            interpolationComponent.CurrentTransform.Rotation = rotation;
            interpolationComponent.PreviousTransform.Rotation = rotation;
            
            translation.position = position;
            interpolationComponent.CurrentTransform.Position = position;
            interpolationComponent.PreviousTransform.Position = position;
            
            interpolationComponent.SkipNextInterpolation();
        }
    }
}