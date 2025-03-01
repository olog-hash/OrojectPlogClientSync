using System.Collections.Generic;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController
{
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct KinematicCharacterBody : IComponent 
	{
        [HideInInspector]
        public CapsuleCollider Capsule;

        public float CapsuleRadius;
        public float CapsuleHeight;
        public float CapsuleYOffset;

        public float GroundDetectionExtraDistance;
        public float MaxStableSlopeAngle;
        public LayerMask StableGroundLayers;

        public bool StepHandling;
        public float MaxStepHeight;
        public float ExtraStepChecksDistance;

        public bool LedgeAndDenivelationHandling;
        public float MaxStableDistanceFromLedge;
        public float MaxVelocityForLedgeSnap;
        public float MaxStableDenivelationAngle;

        public int MaxMovementIterations;
        public int MaxDecollisionIterations;
        public bool CheckMovementInitialOverlaps;
        public bool KillVelocityWhenExceedMaxMovementIterations;
        public bool KillRemainingMovementWhenExceedMaxMovementIterations;

        public LayerMask IgnoreLayers;
        public List<Collider> IgnoredColliders;

        [HideInInspector]
        public bool SolveMovementCollisions;
        [HideInInspector]
        public bool SolveGrounding;

        [HideInInspector]
        public Vector3 BaseVelocity;
        [HideInInspector]
        public CharacterGroundingReport GroundingStatus;
        [HideInInspector]
        public CharacterTransientGroundingReport LastGroundingStatus;
        [HideInInspector]
        public bool LastMovementIterationFoundAnyGround;
        [HideInInspector]
        public bool MustUnground;
        [HideInInspector]
        public float MustUngroundTimeCounter;

        public KinematicCharacterBody(AuthoringKinematicCharacterBody forAuthoring)
        {
            Capsule = forAuthoring.Capsule;

            CapsuleRadius = forAuthoring.CapsuleRadius;
            CapsuleHeight = forAuthoring.CapsuleHeight;
            CapsuleYOffset = forAuthoring.CapsuleYOffset;

            GroundDetectionExtraDistance = forAuthoring.GroundDetectionExtraDistance;
            MaxStableSlopeAngle = forAuthoring.MaxStableSlopeAngle;
            StableGroundLayers = forAuthoring.StableGroundLayers;

            StepHandling = forAuthoring.StepHandling;
            MaxStepHeight = forAuthoring.MaxStepHeight;
            ExtraStepChecksDistance = forAuthoring.ExtraStepChecksDistance;

            LedgeAndDenivelationHandling = forAuthoring.LedgeAndDenivelationHandling;
            MaxStableDistanceFromLedge = forAuthoring.MaxStableDistanceFromLedge;
            MaxVelocityForLedgeSnap = forAuthoring.MaxVelocityForLedgeSnap;
            MaxStableDenivelationAngle = forAuthoring.MaxStableDenivelationAngle;

            MaxMovementIterations = forAuthoring.MaxMovementIterations;
            MaxDecollisionIterations = forAuthoring.MaxDecollisionIterations;
            CheckMovementInitialOverlaps = forAuthoring.CheckMovementInitialOverlaps;
            KillVelocityWhenExceedMaxMovementIterations = forAuthoring.KillVelocityWhenExceedMaxMovementIterations;
            KillRemainingMovementWhenExceedMaxMovementIterations = forAuthoring.KillRemainingMovementWhenExceedMaxMovementIterations;

            IgnoreLayers = forAuthoring.IgnoreLayers;
            IgnoredColliders = forAuthoring.IgnoredColliders;
            SolveMovementCollisions = true;
            SolveGrounding = true;

            BaseVelocity = default;
            GroundingStatus = default;
            LastGroundingStatus = default;
            MustUnground = default;
            MustUngroundTimeCounter = default;
            LastMovementIterationFoundAnyGround = default;
        }

        public void UpdateCapsuleDimensions()
        {
            CapsuleHeight = Mathf.Max(CapsuleHeight, (CapsuleRadius * 2f) + 0.01f); // Safety to prevent invalid capsule geometries

            Capsule.direction = 1;
            Capsule.radius = CapsuleRadius;
            Capsule.height = Mathf.Clamp(CapsuleHeight, CapsuleRadius * 2f, CapsuleHeight);
            Capsule.center = new Vector3(0f, CapsuleYOffset, 0f);

            //MaxStepHeight = Mathf.Clamp(MaxStepHeight, 0f, Mathf.Infinity);
            //ExtraStepChecksDistance = Mathf.Clamp(ExtraStepChecksDistance, 0f, CapsuleRadius);
            //MaxStableDistanceFromLedge = Mathf.Clamp(MaxStableDistanceFromLedge, 0f, CapsuleRadius);
        }

        public void SetCapsuleDimensions(float radius, float height, float yOffset)
        {
            height = Mathf.Max(height, (radius * 2f) + 0.01f); // Safety to prevent invalid capsule geometries 

            CapsuleRadius = radius;
            CapsuleHeight = height;
            CapsuleYOffset = yOffset;

            Capsule.radius = CapsuleRadius;
            Capsule.height = Mathf.Clamp(CapsuleHeight, CapsuleRadius * 2f, CapsuleHeight);
            Capsule.center = new Vector3(0f, CapsuleYOffset, 0f);

            MaxStepHeight = Mathf.Clamp(MaxStepHeight, 0f, Mathf.Infinity);
            ExtraStepChecksDistance = Mathf.Clamp(ExtraStepChecksDistance, 0f, CapsuleRadius);
            MaxStableDistanceFromLedge = Mathf.Clamp(MaxStableDistanceFromLedge, 0f, CapsuleRadius);
        }

        public void ForceUnground(float time = 0.1f)
        {
            MustUnground = true;
            MustUngroundTimeCounter = time;
        }

        public bool WillUnground()
        {
            return MustUnground || MustUngroundTimeCounter > 0f;
        }

        public void SetCollisionDetectionActive(bool active)
        {
            Capsule.isTrigger = !active;
            SolveMovementCollisions = active;
            SolveGrounding = active;
        }
    }
}