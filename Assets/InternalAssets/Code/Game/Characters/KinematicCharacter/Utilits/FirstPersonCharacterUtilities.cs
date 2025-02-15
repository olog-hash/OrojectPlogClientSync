using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits
{
    public static class FirstPersonCharacterUtilities
    {
        public static void ComputeFinalRotationsFromRotationDelta(
            ref Quaternion characterRotation,
            ref float viewYawDegrees,
            ref float viewPitchDegrees,
            Vector2 yawPitchDeltaDegrees,
            float viewRollDegrees,
            float minPitchDegrees,
            float maxPitchDegrees,
            out float canceledPitchDegrees,
            out Quaternion localCharacterViewRotation)
        {
            // Yaw
            Quaternion yawRotation = Quaternion.Euler(Vector3.up * yawPitchDeltaDegrees.x);
            characterRotation *= yawRotation;

            // Pitch
            viewPitchDegrees += yawPitchDeltaDegrees.y;
            float viewPitchAngleDegreesBeforeClamp = viewPitchDegrees;
            viewPitchDegrees = Mathf.Clamp(viewPitchDegrees, minPitchDegrees, maxPitchDegrees);
            canceledPitchDegrees = yawPitchDeltaDegrees.y - (viewPitchAngleDegreesBeforeClamp - viewPitchDegrees);

            localCharacterViewRotation = CalculateLocalViewRotation(viewPitchDegrees, viewRollDegrees);

            viewYawDegrees = Mathf.Clamp(characterRotation.eulerAngles.y, 0f, 360f);
        }

        public static Quaternion CalculateLocalViewRotation(float viewPitchDegrees, float viewRollDegrees)
        {
            // Pitch
            Quaternion viewLocalRotation = Quaternion.AngleAxis(viewPitchDegrees, -Vector3.right);

            // Roll
            viewLocalRotation *= Quaternion.AngleAxis(viewRollDegrees, Vector3.forward);

            return viewLocalRotation;
        }
    }

}
