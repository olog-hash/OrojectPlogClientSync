using ProjectOlog.Code.Networking.Client;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Core
{
    /// <summary>
    /// Система обновления интерполяции в Fixed Update
    /// Сохраняет текущее состояние объектов для последующей интерполяции
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InterpolationFixedUpdateSystem : TickrateSystem
    {
        private Filter filter;
        public double LastFixedUpdateElapsedTime = -1;
        public float LastFixedUpdateTimeStep = 0f;

        public override void OnAwake()
        {
            filter = World.Filter.With<Translation>().With<Interpolation>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in filter)
            {
                ref var translation = ref entity.GetComponent<Translation>();
                var transformInterpolation = entity.GetComponent<Interpolation>();

                // TODO : Пропускать при назначении (skip)

                transformInterpolation.PreviousTransform = transformInterpolation.CurrentTransform;

                //transformInterpolation.CurrentTransform.position = translation.localPosition;
                //transformInterpolation.CurrentTransform.rotation = translation.localRotation;

                entity.SetComponent(transformInterpolation);
            }
        }
    }

    /// <summary>
    /// Система интерполяции в Update
    /// Выполняет плавный переход между предыдущим и текущим состоянием объектов
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InterpolationVariableUpdateSystem : UpdateSystem
    {
        private Filter filter;
        private float NormalizedTimeAhead;

        public override void OnAwake()
        {
            filter = World.Filter.With<Translation>().With<Interpolation>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            float fixedTimeStep = NetworkTime.LastTickUpdateTimeInterval;

            if (NetworkTime.LastTickUpdateElapsedTime <= 0f || fixedTimeStep == 0f)
            {
                return;
            }

            float timeAheadOfLastFixedUpdate = (float)(Time.time - NetworkTime.LastTickUpdateElapsedTime);
            NormalizedTimeAhead = Mathf.Clamp(timeAheadOfLastFixedUpdate / fixedTimeStep, 0f, 1f);

            foreach (Entity entity in filter)
            {
                var transform = entity.GetComponent<Translation>().Transform;
                var transformInterpolation = entity.GetComponent<Interpolation>();

                RigidTransform targetTransform = transformInterpolation.CurrentTransform;

                // Interpolation skipping
                if (transformInterpolation.InterpolationSkipping > 0)
                {
                    if (transformInterpolation.ShouldSkipNextTranslationInterpolation())
                    {
                        transformInterpolation.PreviousTransform.Position = targetTransform.Position;
                    }
                    if (transformInterpolation.ShouldSkipNextRotationInterpolation())
                    {
                        transformInterpolation.PreviousTransform.Rotation = targetTransform.Rotation;
                    }

                    transformInterpolation.InterpolationSkipping = 0;
                    entity.SetComponent(transformInterpolation);
                }

                Vector3 interpolatedPos = targetTransform.Position;
                if (transformInterpolation.InterpolateTranslation)
                {
                    interpolatedPos = Vector3.Lerp(transformInterpolation.PreviousTransform.Position, targetTransform.Position, NormalizedTimeAhead);
                }

                Quaternion interpolatedRot = targetTransform.Rotation;
                if (transformInterpolation.InterpolateRotation)
                {
                    interpolatedRot = Quaternion.Slerp(transformInterpolation.PreviousTransform.Rotation, targetTransform.Rotation, NormalizedTimeAhead);
                }

                transform.localPosition = interpolatedPos;
                transform.localRotation = interpolatedRot;
            }
        }
    }

}