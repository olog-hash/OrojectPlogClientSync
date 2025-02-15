using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterInterpolationFixedUpdateSystem : FixedUpdateSystem
    {
        private Filter filter;
        public double LastFixedUpdateElapsedTime = -1;
        public float LastFixedUpdateTimeStep = 0f;

        public override void OnAwake()
        {
            filter = World.Filter.With<Translation>().With<CharacterInterpolation>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity entity in filter)
            {
                ref var translation = ref entity.GetComponent<Translation>();
                var transformInterpolation = entity.GetComponent<CharacterInterpolation>();

                if (HasComponent<SetPositionRotation>(entity))
                {
                    ref var setPosRot = ref entity.GetComponent<SetPositionRotation>();

                    translation.position = setPosRot.Position;
                    translation.rotation = setPosRot.Rotation;
                    transformInterpolation.SkipNextTranslationInterpolation();

                    entity.RemoveComponent<SetPositionRotation>();
                }

                transformInterpolation.PreviousTransform = transformInterpolation.CurrentTransform;

                entity.SetComponent(transformInterpolation);
            }
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterInterpolationVariableUpdateSystem : UpdateSystem
    {
        private Filter filter;
        private float NormalizedTimeAhead;

        public override void OnAwake()
        {
            filter = World.Filter.With<Translation>().With<CharacterInterpolation>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            float fixedTimeStep = RuntimeHelper.LastFixedUpdateTimeStep;

            if (RuntimeHelper.LastFixedUpdateElapsedTime <= 0f || fixedTimeStep == 0f)
            {
                return;
            }

            float timeAheadOfLastFixedUpdate = (float)(Time.time - RuntimeHelper.LastFixedUpdateElapsedTime);
            NormalizedTimeAhead = Mathf.Clamp(timeAheadOfLastFixedUpdate / fixedTimeStep, 0f, 1f);

            foreach (Entity entity in filter)
            {
                var translation = entity.GetComponent<Translation>();
                var transformInterpolation = entity.GetComponent<CharacterInterpolation>();

                RigidTransform targetTransform = transformInterpolation.CurrentTransform;

                // Interpolation skipping
                if (transformInterpolation.InterpolationSkipping > 0)
                {
                    if (transformInterpolation.ShouldSkipNextTranslationInterpolation())
                    {
                        transformInterpolation.PreviousTransform.position = targetTransform.position;
                    }

                    transformInterpolation.InterpolationSkipping = 0;
                    entity.SetComponent(transformInterpolation);
                }

                Vector3 interpolatedPos = targetTransform.position;
                if (transformInterpolation.InterpolateTranslation)
                {
                    interpolatedPos = Vector3.Lerp(transformInterpolation.PreviousTransform.position, targetTransform.position, NormalizedTimeAhead);
                }

                Quaternion interpolatedRot = targetTransform.rotation;
                if (transformInterpolation.InterpolateRotation)
                {
                    interpolatedRot = Quaternion.Slerp(transformInterpolation.PreviousTransform.rotation, targetTransform.rotation, NormalizedTimeAhead);
                }

                translation.localPosition = interpolatedPos;
                //translation.localRotation = interpolatedRot;
            }
        }
    }


}
