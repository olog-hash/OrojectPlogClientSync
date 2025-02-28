using System.Collections.Generic;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
    [System.Serializable]
    public struct AuthoringKinematicCharacterBody
    {
        [Header("General Properties")]
        [Tooltip("Physics tags to be added to the character's physics body")]
        public LayerMask CustomPhysicsBodyTags;
        [Tooltip("Position relative to the axis of which coordinates - world and local (if the entity is a child)")]
        public bool InterpolateGlobal;
        [Tooltip("Enables interpolating the character's translation between fixed update steps, for smoother movement")]
        public bool InterpolateTranslation;
        [Tooltip("Enables interpolating the character's rotation between fixed update steps, for smoother movement")]
        public bool InterpolateRotation;

        [Header("Capsule Settings")]
        [Tooltip("Radius of the Character Capsule")]
        public float CapsuleRadius;
        [Tooltip("Height of the Character Capsule")]
        public float CapsuleHeight;
        [Tooltip("Height of the Character Capsule")]
        public float CapsuleYOffset;

        [Header("Grounding settings")]
        [Tooltip("Increases the range of ground detection, to allow snapping to ground at very high speeds")]
        public float GroundDetectionExtraDistance;
        [Range(0f, 89f)]
        [Tooltip("Maximum slope angle on which the character can be stable")]
        public float MaxStableSlopeAngle;
        [Tooltip("Which layers can the character be considered stable on")]
        public LayerMask StableGroundLayers;

        [Header("Step settings")]
        [Tooltip("Handles properly detecting grounding status on steps, but has a performance cost.")]
        public bool StepHandling;
        [Tooltip("Maximum height of a step which the character can climb")]
        public float MaxStepHeight;
        [Tooltip("Minimum length of a step that the character can step on (used in Extra stepping method). Use this to let the character step on steps that are smaller that its radius")]
        public float ExtraStepChecksDistance;

        [Header("Ledge settings")]
        [Tooltip("Handles properly detecting ledge information and grounding status, but has a performance cost.")]
        public bool LedgeAndDenivelationHandling;
        [Tooltip("The distance from the capsule central axis at which the character can stand on a ledge and still be stable")]
        public float MaxStableDistanceFromLedge;
        [Tooltip("Prevents snapping to ground on ledges beyond a certain velocity")]
        public float MaxVelocityForLedgeSnap;
        [Tooltip("The maximun downward slope angle change that the character can be subjected to and still be snapping to the ground")]
        [Range(1f, 180f)]
        public float MaxStableDenivelationAngle;

        [Header("Other settings")]
        [Tooltip("How many times can we sweep for movement per update")]
        public int MaxMovementIterations;
        [Tooltip("How many times can we check for decollision per update")]
        public int MaxDecollisionIterations;
        [Tooltip("Checks for overlaps before casting movement, making sure all collisions are detected even when already intersecting geometry (has a performance cost, but provides safety against tunneling through colliders)")]
        public bool CheckMovementInitialOverlaps;
        [Tooltip("Sets the velocity to zero if exceed max movement iterations")]
        public bool KillVelocityWhenExceedMaxMovementIterations;
        [Tooltip("Sets the remaining movement to zero if exceed max movement iterations")]
        public bool KillRemainingMovementWhenExceedMaxMovementIterations;

        public LayerMask IgnoreLayers;
        [Tooltip("Colliders that the controller will ignore")]
        public List<Collider> IgnoredColliders;

        [HideInInspector]
        public CapsuleCollider Capsule;

        public static AuthoringKinematicCharacterBody GetDefault()
        {
            AuthoringKinematicCharacterBody c = new AuthoringKinematicCharacterBody
            {
                // Body Properties
                CustomPhysicsBodyTags = -1,
                InterpolateTranslation = true,
                InterpolateRotation = false,

                // Capsule
                CapsuleRadius = 0.5f,
                CapsuleHeight = 2f,
                CapsuleYOffset = 1f,

                // Grounding settings
                GroundDetectionExtraDistance = 0,
                MaxStableSlopeAngle = 60,
                StableGroundLayers = -1,

                // Step settings
                StepHandling = true,
                MaxStepHeight = 0.5f,
                ExtraStepChecksDistance = 0f,

                // Ledge settings
                LedgeAndDenivelationHandling = true,
                MaxStableDistanceFromLedge = 0.5f,
                MaxVelocityForLedgeSnap = 0,
                MaxStableDenivelationAngle = 180,

                MaxMovementIterations = 5,
                MaxDecollisionIterations = 1,
                CheckMovementInitialOverlaps = true,
                KillVelocityWhenExceedMaxMovementIterations = true,
                KillRemainingMovementWhenExceedMaxMovementIterations = true,
                
                IgnoredColliders = new List<Collider>(),
            };

            return c;
        }
    }

}
