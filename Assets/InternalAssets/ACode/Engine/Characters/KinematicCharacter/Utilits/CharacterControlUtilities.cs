using System.Runtime.CompilerServices;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits
{
    public static class CharacterControlUtilities
    {
        // calculates the signed slope angle (radians) in a given movement direction.
        // The resulting angle will be positive if the slope goes up, and negative if the slope goes down.
        public static float GetSlopeAngleTowardsDirection(bool useDegrees, Vector3 moveDirection, Vector3 slopeNormal, Vector3 groundingUp)
        {
            float3 moveDirectionOnSlopePlane = math.normalizesafe(MathUtilities.ProjectOnPlane(moveDirection, slopeNormal));
            float angleRadiansWithUp = MathUtilities.AngleRadians(moveDirectionOnSlopePlane, groundingUp);

            if (useDegrees)
            {
                return 90f - math.degrees(angleRadiansWithUp);
            }
            else
            {
                return (math.PI * 0.5f) - angleRadiansWithUp;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StandardGroundMove_Interpolated(ref Vector3 velocity, Vector3 targetVelocity, float sharpness, float deltaTime, Vector3 groundingUp, Vector3 groundedHitNormal)
        {
            velocity = MathUtilities.ReorientVectorOnPlaneAlongDirection(velocity, groundedHitNormal, groundingUp);
            targetVelocity = MathUtilities.ReorientVectorOnPlaneAlongDirection(targetVelocity, groundedHitNormal, groundingUp);
            InterpolateVelocityTowardsTarget(ref velocity, targetVelocity, deltaTime, sharpness);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StandardGroundMove_Accelerated(ref Vector3 velocity, Vector3 acceleration, float maxSpeed, float deltaTime, Vector3 movementPlaneUp, Vector3 groundedHitNormal, bool forceNoSpeedExcess)
        {
            Vector3 addedVelocityFromAcceleration = float3.zero;
            AccelerateVelocity(ref addedVelocityFromAcceleration, acceleration, deltaTime);

            velocity = MathUtilities.ReorientVectorOnPlaneAlongDirection(velocity, groundedHitNormal, movementPlaneUp);
            addedVelocityFromAcceleration = MathUtilities.ReorientVectorOnPlaneAlongDirection(addedVelocityFromAcceleration, groundedHitNormal, movementPlaneUp);
            ClampAdditiveVelocityToMaxSpeedOnPlane(ref addedVelocityFromAcceleration, velocity, maxSpeed, groundedHitNormal, forceNoSpeedExcess);
            velocity += addedVelocityFromAcceleration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StandardAirMove(ref Vector3 velocity, Vector3 acceleration, float maxSpeed, Vector3 movementPlaneUp, float deltaTime, bool forceNoMaxSpeedExcess)
        {
            Vector3 addedVelocityFromAcceleration = float3.zero;
            AccelerateVelocity(ref addedVelocityFromAcceleration, acceleration, deltaTime);
            ClampAdditiveVelocityToMaxSpeedOnPlane(ref addedVelocityFromAcceleration, velocity, maxSpeed, movementPlaneUp, forceNoMaxSpeedExcess);
            velocity += addedVelocityFromAcceleration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InterpolateVelocityTowardsTarget(ref Vector3 velocity, Vector3 targetVelocity, float deltaTime, float interpolationSharpness)
        {
            velocity = math.lerp(velocity, targetVelocity, MathUtilities.GetSharpnessInterpolant(interpolationSharpness, deltaTime));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AccelerateVelocity(ref Vector3 velocity, Vector3 acceleration, float deltaTime)
        {
            velocity += acceleration * deltaTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampAdditiveVelocityToMaxSpeedOnPlane(ref Vector3 additiveVelocity, Vector3 originalVelocity, float maxSpeed, Vector3 movementPlaneUp, bool forceNoMaxSpeedExcess)
        {
            if (forceNoMaxSpeedExcess)
            {
                Vector3 totalVelocity = originalVelocity + additiveVelocity;
                Vector3 velocityUp = math.projectsafe(totalVelocity, movementPlaneUp);
                Vector3 velocityHorizontal = MathUtilities.ProjectOnPlane(totalVelocity, movementPlaneUp);
                velocityHorizontal = MathUtilities.ClampToMaxLength(velocityHorizontal, maxSpeed);
                additiveVelocity = (velocityHorizontal + velocityUp) - originalVelocity;
            }
            else
            {
                float maxSpeedSq = maxSpeed * maxSpeed;

                Vector3 additiveVelocityOnPlaneUp = math.projectsafe(additiveVelocity, movementPlaneUp);
                Vector3 additiveVelocityOnPlane = additiveVelocity - additiveVelocityOnPlaneUp;

                Vector3 originalVelocityOnPlaneUp = math.projectsafe(originalVelocity, movementPlaneUp);
                Vector3 originalVelocityOnPlane = originalVelocity - originalVelocityOnPlaneUp;
                Vector3 totalVelocityOnPlane = originalVelocityOnPlane + additiveVelocityOnPlane;

                if (math.lengthsq(totalVelocityOnPlane) > maxSpeedSq)
                {
                    Vector3 originalVelocityForwardOnPlane = math.normalizesafe(originalVelocityOnPlane);
                    Vector3 totalVelocityDirectionOnPlane = math.normalizesafe(totalVelocityOnPlane);

                    Vector3 totalClampedVelocityOnPlane = float3.zero;
                    if (math.dot(totalVelocityDirectionOnPlane, originalVelocityForwardOnPlane) > 0f)
                    {
                        Vector3 originalVelocityRightOnPlane = math.normalizesafe(math.cross(originalVelocityForwardOnPlane, movementPlaneUp));

                        // trim additive velocity excess in original velocity direction
                        Vector3 trimmedTotalVelocityForwardComponent = MathUtilities.ClampToMaxLength(math.projectsafe(totalVelocityOnPlane, originalVelocityForwardOnPlane), math.max(maxSpeed, math.length(originalVelocityOnPlane)));
                        Vector3 trimmedTotalVelocityRightComponent = MathUtilities.ClampToMaxLength(math.projectsafe(totalVelocityOnPlane, originalVelocityRightOnPlane), maxSpeed);
                        totalClampedVelocityOnPlane = trimmedTotalVelocityForwardComponent + trimmedTotalVelocityRightComponent;
                    }
                    else
                    {
                        // clamp totalvelocity to circle
                        totalClampedVelocityOnPlane = MathUtilities.ClampToMaxLength(totalVelocityOnPlane, maxSpeed);
                    }

                    Vector3 clampedAdditiveVelocityOnPlane = totalClampedVelocityOnPlane - originalVelocityOnPlane;
                    additiveVelocity = clampedAdditiveVelocityOnPlane + additiveVelocityOnPlaneUp;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StandardJump(ref KinematicCharacterBody characterBody, Vector3 jumpVelocity, bool cancelVelocityBeforeJump = false, float3 velocityCancelingUpDirection = default)
        {
            // Without this, the ground snapping mecanism would prevent you from jumping
            characterBody.ForceUnground();

            if (cancelVelocityBeforeJump)
            {
                characterBody.BaseVelocity = MathUtilities.ProjectOnPlane(characterBody.BaseVelocity, velocityCancelingUpDirection);
            }

            characterBody.BaseVelocity += jumpVelocity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ApplyDragToVelocity(ref Vector3 velocity, float deltaTime, float drag)
        {
            velocity *= (1f / (1f + (drag * deltaTime)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ApplyWeaponPushToInternalVelocity(ref Vector3 internalVelocityAdd, Vector3 direction, Vector2 power)
        {
            direction.x *= power.x;
            direction.y *= power.y;
            direction.z *= power.x;

            internalVelocityAdd -= direction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetLinearVelocityForMovePosition(float deltaTime, Vector3 positionDelta)
        {
            if (deltaTime > 0f)
            {
                return positionDelta / deltaTime;
            }
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SlerpRotationTowardsDirectionAroundUp(ref quaternion rotation, float deltaTime, Vector3 worldTargetDirection, Vector3 upDirection, float orientationSharpness)
        {
            if (math.lengthsq(worldTargetDirection) > 0f)
            {
                rotation = math.slerp(rotation, MathUtilities.CreateRotationWithUpPriority(upDirection, worldTargetDirection), MathUtilities.GetSharpnessInterpolant(orientationSharpness, deltaTime));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CameraPositionUpdate(ref float transientCameraHeight, bool isCrouch, float deltaTime)
        {
            float cameraPoint = isCrouch ? KinematicCharacterUtilities.Constants.MinCameraHeight : KinematicCharacterUtilities.Constants.MaxCameraHeight;

            transientCameraHeight = Mathf.MoveTowards(transientCameraHeight, cameraPoint, KinematicCharacterUtilities.Constants.CameraHeightStep * deltaTime);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InternalVelocityApply(ref KinematicCharacterBody kinematicCharacterBody, ref Vector3 internalVelocityAdd)
        {
            if (internalVelocityAdd != Vector3.zero)
            {
                kinematicCharacterBody.BaseVelocity += internalVelocityAdd;
                kinematicCharacterBody.ForceUnground();

                internalVelocityAdd = Vector3.zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetMovementSpeed(FirstPersonCharacter firstPersonCharacter, FirstPersonInputs firstPersonCharacterInputs, out float groundMaxSpeed)
        {
            groundMaxSpeed = (firstPersonCharacterInputs.SprintRequested) ? firstPersonCharacter.SprintSpeed : (firstPersonCharacterInputs.CrouchRequested) ? firstPersonCharacter.CrouchSpeed : firstPersonCharacter.WalkSpeed;
            //groundMaxSpeed = firstPersonCharacter.IsAlternativeMode ? firstPersonCharacter.AlternativeModeSpeed : groundMaxSpeed;
        }
    }
}
