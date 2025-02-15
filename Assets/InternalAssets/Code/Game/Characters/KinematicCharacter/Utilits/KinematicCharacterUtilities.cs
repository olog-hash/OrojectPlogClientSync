using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits
{
    public enum MovementSweepState
    {
        Initial,
        AfterFirstHit,
        FoundBlockingCrease,
        FoundBlockingCorner,
    }

    public struct CharacterGroundingReport
    {
        public bool FoundAnyGround;
        public bool IsStableOnGround;
        public bool SnappingPrevented;
        public Vector3 GroundNormal;
        public Vector3 InnerGroundNormal;
        public Vector3 OuterGroundNormal;

        public Collider GroundCollider;
        public Vector3 GroundPoint;

        public void CopyFrom(CharacterTransientGroundingReport transientGroundingReport)
        {
            FoundAnyGround = transientGroundingReport.FoundAnyGround;
            IsStableOnGround = transientGroundingReport.IsStableOnGround;
            SnappingPrevented = transientGroundingReport.SnappingPrevented;
            GroundNormal = transientGroundingReport.GroundNormal;
            InnerGroundNormal = transientGroundingReport.InnerGroundNormal;
            OuterGroundNormal = transientGroundingReport.OuterGroundNormal;

            GroundCollider = null;
            GroundPoint = Vector3.zero;
        }
    }

    public struct CharacterTransientGroundingReport
    {
        public bool FoundAnyGround;
        public bool IsStableOnGround;
        public bool SnappingPrevented;
        public Vector3 GroundNormal;
        public Vector3 InnerGroundNormal;
        public Vector3 OuterGroundNormal;

        public void CopyFrom(CharacterGroundingReport groundingReport)
        {
            FoundAnyGround = groundingReport.FoundAnyGround;
            IsStableOnGround = groundingReport.IsStableOnGround;
            SnappingPrevented = groundingReport.SnappingPrevented;
            GroundNormal = groundingReport.GroundNormal;
            InnerGroundNormal = groundingReport.InnerGroundNormal;
            OuterGroundNormal = groundingReport.OuterGroundNormal;
        }
    }

    public struct OverlapResult
    {
        public Vector3 Normal;
        public Collider Collider;

        public OverlapResult(Vector3 normal, Collider collider)
        {
            Normal = normal;
            Collider = collider;
        }
    }

    public struct HitStabilityReport
    {
        public bool IsStable;

        public bool FoundInnerNormal;
        public Vector3 InnerNormal;
        public bool FoundOuterNormal;
        public Vector3 OuterNormal;

        public bool ValidStepDetected;
        public Collider SteppedCollider;

        public bool LedgeDetected;
        public bool IsOnEmptySideOfLedge;
        public float DistanceFromLedge;
        public bool IsMovingTowardsEmptySideOfLedge;
        public Vector3 LedgeGroundNormal;
        public Vector3 LedgeRightDirection;
        public Vector3 LedgeFacingDirection;
    }


    public interface IKinematicCharacterProcessor
    {
        public float GetDeltaTime { get; }

        bool CharacterGroundSweep(
            Vector3 groundSweepPosition, // position
            Quaternion atRotation, // rotation
            Vector3 groundSweepDirection, // direction
            float groundProbeDistanceRemaining, // distance
            out RaycastHit groundSweepHit);

        public void EvaluateHitStability(
            Collider hitCollider,
            Vector3 hitNormal,
            Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation,
            Vector3 withCharacterVelocity,
            ref HitStabilityReport stabilityReport);

        public bool CheckStepValidity(
            int nbStepHits,
            Vector3 characterPosition,
            Quaternion characterRotation,
            Vector3 innerHitDirection,
            Vector3 stepCheckStartPos,
            out Collider hitCollider);

        public bool IsColliderValidForCollisions(Collider coll);
        
        public bool IsInIgnoreLayer(Collider coll);
    }

    public static class KinematicCharacterUtilities
    {
        public struct Constants
        {
            // Warning: Don't touch these constants unless you know exactly what you're doing!
            public const int MaxHitsBudget = 16;
            public const int MaxCollisionBudget = 16;
            public const int MaxGroundingSweepIterations = 2;
            public const int MaxSteppingSweepIterations = 3;
            public const int MaxRigidbodyOverlapsCount = 16;
            public const float CollisionOffset = 0.01f;
            public const float GroundProbeReboundDistance = 0.02f;
            public const float MinimumGroundProbingDistance = 0.005f;
            public const float GroundProbingBackstepDistance = 0.1f;
            public const float SweepProbingBackstepDistance = 0.002f;
            public const float SecondaryProbesVertical = 0.02f;
            public const float SecondaryProbesHorizontal = 0.001f;
            public const float MinVelocityMagnitude = 0.01f;
            public const float SteppingForwardDistance = 0.03f;
            public const float MinDistanceForLedge = 0.05f;
            public const float CorrelationForVerticalObstruction = 0.01f;
            public const float ExtraSteppingForwardDistance = 0.01f;
            public const float ExtraStepHeightPadding = 0.01f;
            public const float GroundDetectionExtraDistance = 0f;
            public const float MaxCameraHeight = 1.6f;
            public const float MinCameraHeight = 1.1f;
            public const float CameraHeightStep = 3.5f;
        }


        // Adds all the required character components to an entity
        public static void CreateCharacter(
            Entity entity,
            Transform transform,
            AuthoringKinematicCharacterBody authoringProperties)
        {
            // Base character components
            //var firstCharacterStateMachine = new FirstCharacterStateMachine();
            //firstCharacterStateMachine.CurrentCharacterState = CharacterState.GroundMove;

            entity.AddComponentData(new KinematicCharacterBody(authoringProperties));
            entity.AddComponent<CharacterBodyLogger>();
            entity.AddComponentData(FirstCharacterStateMachine.GetDefault());

            // Interpolation
            if (authoringProperties.InterpolateTranslation || authoringProperties.InterpolateRotation)
            {
                entity.AddComponentData(new CharacterInterpolation
                {
                    InterpolateTranslation = authoringProperties.InterpolateTranslation,

                    CurrentTransform = new RigidTransform(transform.localRotation, transform.localPosition),
                });
            }
        }

        // Handles the conversion from GameObject to Entity for a character
        public static void HandleConversionForCharacter(
            Entity entity,
            UnityEngine.GameObject authoringGameObject,
            AuthoringKinematicCharacterBody authoringProperties)
        {
            if (authoringGameObject.transform.lossyScale != UnityEngine.Vector3.one)
            {
                UnityEngine.Debug.LogError("ERROR: kinematic character objects do not support having a scale other than (1,1,1). Conversion will be aborted");
                return;
            }

            CreateCharacter(entity, authoringGameObject.transform, authoringProperties);
        }

        /// <summary>
        ///  clears some core character variables and buffers at the start of the update.
        /// </summary>
        public static void InitializationUpdate(
            ref KinematicCharacterBody characterBody,
            ref RaycastHit[] InternalCharacterHits,
            ref Collider[] InternalProbedColliders,
            ref OverlapResult[] Overlaps,
            ref int OverlapsCount)
        {
            // Initialize data for update
            InternalCharacterHits = new RaycastHit[Constants.MaxHitsBudget];
            InternalProbedColliders = new Collider[Constants.MaxCollisionBudget];
            Overlaps = new OverlapResult[Constants.MaxRigidbodyOverlapsCount];
            OverlapsCount = 0;

            characterBody.UpdateCapsuleDimensions();
            characterBody.LastGroundingStatus.CopyFrom(characterBody.GroundingStatus);
            characterBody.GroundingStatus = new CharacterGroundingReport();
            characterBody.GroundingStatus.GroundNormal = Vector3.up;
        }

        public static void SolvingCollisionOverlaps(
            IKinematicCharacterProcessor processor,
            ref Vector3 TransientPosition,
            ref Collider[] InternalProbedColliders,
            ref OverlapResult[] Overlaps,
            ref int OverlapsCount,
            Vector3 GroundingUp,
            Quaternion TransientRotation,
            KinematicCharacterBody characterBody)
        {
            if (characterBody.SolveMovementCollisions)
            {
                Vector3 resolutionDirection = Vector3.up;
                float resolutionDistance = 0f;
                int iterationsMade = 0;
                bool overlapSolved = false;
                while (iterationsMade < characterBody.MaxDecollisionIterations && !overlapSolved)
                {
                    int nbOverlaps = CharacterCollisionsOverlap(processor, characterBody.Capsule, TransientPosition, TransientRotation, ref InternalProbedColliders);

                    if (nbOverlaps > 0)
                    {
                        // Solve overlaps that aren't against dynamic rigidbodies or physics movers
                        for (int i = 0; i < nbOverlaps; i++)
                        {
                            // Process overlap
                            Transform overlappedTransform = InternalProbedColliders[i].GetComponent<Transform>();
                            if (UnityEngine.Physics.ComputePenetration(
                                    characterBody.Capsule,
                                    TransientPosition,
                                    TransientRotation,
                                    InternalProbedColliders[i],
                                    overlappedTransform.position,
                                    overlappedTransform.rotation,
                                    out resolutionDirection,
                                    out resolutionDistance))
                            {
                                // Resolve along obstruction direction
                                HitStabilityReport mockReport = new HitStabilityReport();
                                mockReport.IsStable = IsStableOnNormal(resolutionDirection, GroundingUp, characterBody.MaxStableSlopeAngle);
                                resolutionDirection = GetObstructionNormal(characterBody, GroundingUp, resolutionDirection, mockReport.IsStable);

                                // Solve overlap
                                Vector3 resolutionMovement = resolutionDirection * (resolutionDistance + Constants.CollisionOffset);
                                TransientPosition += resolutionMovement;

                                // Remember overlaps
                                if (OverlapsCount < Overlaps.Length)
                                {
                                    Overlaps[OverlapsCount] = new OverlapResult(resolutionDirection, InternalProbedColliders[i]);
                                    OverlapsCount++;
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        overlapSolved = true;
                    }

                    iterationsMade++;
                }
            }
        }

        public static void GroundingUpdate<T>(
            ref T processor,
            ref Vector3 TransientPosition,
            ref KinematicCharacterBody characterBody,
            Quaternion TransientRotation,
            Vector3 GroundingUp) where T : struct, IKinematicCharacterProcessor
        {
            // Handle ungrounding
            if (characterBody.SolveGrounding)
            {
                if (characterBody.WillUnground())
                {
                    TransientPosition += GroundingUp * (Constants.MinimumGroundProbingDistance * 1.5f);
                }
                else
                {
                    // Choose the appropriate ground probing distance
                    float selectedGroundProbingDistance = Constants.MinimumGroundProbingDistance;
                    if (!characterBody.LastGroundingStatus.SnappingPrevented && (characterBody.LastGroundingStatus.IsStableOnGround || characterBody.LastMovementIterationFoundAnyGround))
                    {
                        if (characterBody.StepHandling)
                        {
                            selectedGroundProbingDistance = Mathf.Max(characterBody.CapsuleRadius, characterBody.MaxStepHeight);
                        }
                        else
                        {
                            selectedGroundProbingDistance = characterBody.CapsuleRadius;
                        }

                        selectedGroundProbingDistance += characterBody.GroundDetectionExtraDistance;
                    }

                    ProbeGround(ref processor, ref TransientPosition, TransientRotation, selectedGroundProbingDistance, ref characterBody.GroundingStatus, characterBody);

                    if (!characterBody.LastGroundingStatus.IsStableOnGround && characterBody.GroundingStatus.IsStableOnGround)
                    {
                        // Handle stable landing
                        characterBody.BaseVelocity = Vector3.ProjectOnPlane(characterBody.BaseVelocity, GroundingUp);
                        characterBody.BaseVelocity = GetDirectionTangentToSurface(characterBody.BaseVelocity, characterBody.GroundingStatus.GroundNormal, GroundingUp) * characterBody.BaseVelocity.magnitude;
                    }
                }
            }

            characterBody.LastMovementIterationFoundAnyGround = false;

            if (characterBody.MustUngroundTimeCounter > 0f)
            {
                characterBody.MustUngroundTimeCounter -= processor.GetDeltaTime;
            }
            characterBody.MustUnground = false;
        }


        /// <summary>
        /// Probes for valid ground and midifies the input transientPosition if ground snapping occurs
        /// </summary>
        public static void ProbeGround<T>(
            ref T processor,
            ref Vector3 probingPosition,
            Quaternion atRotation,
            float probingDistance,
            ref CharacterGroundingReport groundingReport,
            KinematicCharacterBody characterBody) where T : struct, IKinematicCharacterProcessor
        {
            if (probingDistance < Constants.MinimumGroundProbingDistance)
            {
                probingDistance = Constants.MinimumGroundProbingDistance;
            }

            int groundSweepsMade = 0;
            RaycastHit groundSweepHit = new RaycastHit();
            bool groundSweepingIsOver = false;
            Vector3 groundSweepPosition = probingPosition;
            Vector3 groundSweepDirection = (atRotation * -Vector3.up);
            Quaternion transientRotation = atRotation;
            float groundProbeDistanceRemaining = probingDistance;
            while (groundProbeDistanceRemaining > 0 && (groundSweepsMade <= Constants.MaxGroundingSweepIterations) && !groundSweepingIsOver)
            {
                // Sweep for ground detection
                if (processor.CharacterGroundSweep(
                        groundSweepPosition, // position
                        atRotation, // rotation
                        groundSweepDirection, // direction
                        groundProbeDistanceRemaining, // distance
                        out groundSweepHit)) // hit
                {
                    Vector3 targetPosition = groundSweepPosition + (groundSweepDirection * groundSweepHit.distance);
                    HitStabilityReport groundHitStabilityReport = new HitStabilityReport();
                    processor.EvaluateHitStability(groundSweepHit.collider, groundSweepHit.normal, groundSweepHit.point, targetPosition, transientRotation, characterBody.BaseVelocity, ref groundHitStabilityReport);

                    groundingReport.FoundAnyGround = true;
                    groundingReport.GroundNormal = groundSweepHit.normal;
                    groundingReport.InnerGroundNormal = groundHitStabilityReport.InnerNormal;
                    groundingReport.OuterGroundNormal = groundHitStabilityReport.OuterNormal;
                    groundingReport.GroundCollider = groundSweepHit.collider;
                    groundingReport.GroundPoint = groundSweepHit.point;
                    groundingReport.SnappingPrevented = false;

                    // Found stable ground
                    if (groundHitStabilityReport.IsStable)
                    {
                        // Find all scenarios where ground snapping should be canceled
                        groundingReport.SnappingPrevented = !IsStableWithSpecialCases(ref groundHitStabilityReport, characterBody.BaseVelocity, characterBody);

                        groundingReport.IsStableOnGround = true;

                        // Ground snapping
                        if (!groundingReport.SnappingPrevented)
                        {
                            probingPosition = groundSweepPosition + (groundSweepDirection * (groundSweepHit.distance - Constants.CollisionOffset));
                        }

                        groundSweepingIsOver = true;
                    }
                    else
                    {
                        // Calculate movement from this iteration and advance position
                        Vector3 sweepMovement = (groundSweepDirection * groundSweepHit.distance) + ((atRotation * Vector3.up) * Mathf.Max(Constants.CollisionOffset, groundSweepHit.distance));
                        groundSweepPosition = groundSweepPosition + sweepMovement;

                        // Set remaining distance
                        groundProbeDistanceRemaining = Mathf.Min(Constants.GroundProbeReboundDistance, Mathf.Max(groundProbeDistanceRemaining - sweepMovement.magnitude, 0f));

                        // Reorient direction
                        groundSweepDirection = Vector3.ProjectOnPlane(groundSweepDirection, groundSweepHit.normal).normalized;
                    }
                }
                else
                {
                    groundSweepingIsOver = true;
                }

                groundSweepsMade++;
            }
        }

        public static void MovementAndDecollisionsUpdate<T>(
            ref T processor,
            ref KinematicCharacterBody characterBody,
            ref Vector3 transientPosition,
            Quaternion transientRotation,
            Vector3 groundingUp,
            ref RaycastHit[] internalCharacterHits,
            ref Collider[] internalProbedColliders,
            ref OverlapResult[] overlaps,
            int overlapsCount,
            float deltaTime) where T : struct, IKinematicCharacterProcessor
        {

            if (characterBody.SolveMovementCollisions)
            {
                Vector3 resolutionDirection = Vector3.up;
                float resolutionDistance = 0f;
                int iterationsMade = 0;
                bool overlapSolved = false;
                while (iterationsMade < characterBody.MaxDecollisionIterations && !overlapSolved)
                {
                    int nbOverlaps = CharacterCollisionsOverlap(processor, characterBody.Capsule, transientPosition, transientRotation, ref internalProbedColliders);
                    if (nbOverlaps > 0)
                    {
                        for (int i = 0; i < nbOverlaps; i++)
                        {
                            // Process overlap
                            Transform overlappedTransform = internalProbedColliders[i].GetComponent<Transform>();
                            if (UnityEngine.Physics.ComputePenetration(
                                    characterBody.Capsule,
                                    transientPosition,
                                    transientRotation,
                                    internalProbedColliders[i],
                                    overlappedTransform.position,
                                    overlappedTransform.rotation,
                                    out resolutionDirection,
                                    out resolutionDistance))
                            {
                                // Resolve along obstruction direction
                                HitStabilityReport mockReport = new HitStabilityReport();
                                mockReport.IsStable = IsStableOnNormal(resolutionDirection, groundingUp, characterBody.MaxStableSlopeAngle);
                                resolutionDirection = GetObstructionNormal(characterBody, groundingUp, resolutionDirection, mockReport.IsStable);

                                // Solve overlap 
                                Vector3 resolutionMovement = resolutionDirection * (resolutionDistance + Constants.CollisionOffset);
                                transientPosition += resolutionMovement;

                                // Remember overlaps
                                if (overlapsCount < overlaps.Length)
                                {
                                    overlaps[overlapsCount] = new OverlapResult(resolutionDirection, internalProbedColliders[i]);
                                    overlapsCount++;
                                }

                                break;
                            }
                        }
                    }
                    else
                    {
                        overlapSolved = true;
                    }

                    iterationsMade++;
                }
            }

            //this.CharacterController.UpdateVelocity(ref BaseVelocity, deltaTime);
            if (characterBody.BaseVelocity.magnitude < Constants.MinVelocityMagnitude)
            {
                characterBody.BaseVelocity = Vector3.zero;
            }

            // Perform the move from base velocity
            if (characterBody.BaseVelocity.sqrMagnitude > 0f)
            {
                if (characterBody.SolveMovementCollisions)
                {
                    Vector3 velocity = characterBody.BaseVelocity;

                    InternalCharacterMove(
                        ref processor,
                        ref characterBody,
                        ref transientPosition,
                        transientRotation,
                        ref velocity,
                        groundingUp,
                        ref internalCharacterHits,
                        ref internalProbedColliders,
                        ref overlaps,
                        ref overlapsCount,
                        deltaTime);
                }
                else
                {
                    transientPosition += characterBody.BaseVelocity * deltaTime;
                }
            }
        }

        /// <summary>
        /// Moves the character's position by given movement while taking into account all physics simulation, step-handling and 
        /// velocity projection rules that affect the character motor
        /// </summary>
        /// <returns> Returns false if movement could not be solved until the end </returns>
        private static bool InternalCharacterMove<T>(
            ref T processor,
            ref KinematicCharacterBody characterBody,
            ref Vector3 transientPosition,
            Quaternion transientRotation,
            ref Vector3 transientVelocity,
            Vector3 groundingUp,
            ref RaycastHit[] internalCharacterHits,
            ref Collider[] internalProbedColliders,
            ref OverlapResult[] overlaps,
            ref int overlapsCount,
            float deltaTime) where T : struct, IKinematicCharacterProcessor
        {
            if (deltaTime <= 0f)
                return false;

            bool wasCompleted = true;
            Vector3 remainingMovementDirection = transientVelocity.normalized;
            float remainingMovementMagnitude = transientVelocity.magnitude * deltaTime;
            Vector3 originalVelocityDirection = remainingMovementDirection;
            int sweepsMade = 0;
            bool hitSomethingThisSweepIteration = true;
            Vector3 tmpMovedPosition = transientPosition;
            bool previousHitIsStable = false;
            Vector3 previousVelocity = Vector3.zero;
            Vector3 previousObstructionNormal = Vector3.zero;
            MovementSweepState sweepState = MovementSweepState.Initial;

            // Project movement against current overlaps before doing the sweeps
            for (int i = 0; i < overlapsCount; i++)
            {
                Vector3 overlapNormal = overlaps[i].Normal;
                if (Vector3.Dot(remainingMovementDirection, overlapNormal) < 0f)
                {
                    bool stableOnHit = IsStableOnNormal(overlapNormal, groundingUp, characterBody.MaxStableSlopeAngle) && !characterBody.WillUnground();
                    Vector3 velocityBeforeProjection = transientVelocity;
                    Vector3 obstructionNormal = GetObstructionNormal(characterBody, groundingUp, overlapNormal, stableOnHit);

                    InternalHandleVelocityProjection(
                        stableOnHit,
                        overlapNormal,
                        obstructionNormal,
                        originalVelocityDirection,
                        ref sweepState,
                        previousHitIsStable,
                        previousVelocity,
                        previousObstructionNormal,
                        ref transientVelocity,
                        ref remainingMovementMagnitude,
                        ref remainingMovementDirection,
                        ref characterBody,
                        groundingUp);

                    previousHitIsStable = stableOnHit;
                    previousVelocity = velocityBeforeProjection;
                    previousObstructionNormal = obstructionNormal;
                }
            }

            // Sweep the desired movement to detect collisions
            while (remainingMovementMagnitude > 0f &&
                (sweepsMade <= characterBody.MaxMovementIterations) &&
                hitSomethingThisSweepIteration)
            {
                bool foundClosestHit = false;
                Vector3 closestSweepHitPoint = default;
                Vector3 closestSweepHitNormal = default;
                float closestSweepHitDistance = 0f;
                Collider closestSweepHitCollider = null;

                if (characterBody.CheckMovementInitialOverlaps)
                {
                    int numOverlaps = CharacterCollisionsOverlap(
                                        processor,
                                        characterBody.Capsule,
                                        tmpMovedPosition,
                                        transientRotation,
                                        ref internalProbedColliders,
                                        0f,
                                        false);
                    if (numOverlaps > 0)
                    {
                        closestSweepHitDistance = 0f;

                        float mostObstructingOverlapNormalDotProduct = 2f;

                        for (int i = 0; i < numOverlaps; i++)
                        {
                            Collider tmpCollider = internalProbedColliders[i];

                            if (UnityEngine.Physics.ComputePenetration(
                                characterBody.Capsule,
                                tmpMovedPosition,
                                transientRotation,
                                tmpCollider,
                                tmpCollider.transform.position,
                                tmpCollider.transform.rotation,
                                out Vector3 resolutionDirection,
                                out float resolutionDistance))
                            {
                                float dotProduct = Vector3.Dot(remainingMovementDirection, resolutionDirection);
                                if (dotProduct < 0f && dotProduct < mostObstructingOverlapNormalDotProduct)
                                {
                                    mostObstructingOverlapNormalDotProduct = dotProduct;

                                    closestSweepHitNormal = resolutionDirection;
                                    closestSweepHitCollider = tmpCollider;
                                    closestSweepHitPoint = tmpMovedPosition + (transientRotation * characterBody.Capsule.center) + (resolutionDirection * resolutionDistance);

                                    foundClosestHit = true;
                                }
                            }
                        }
                    }
                }

                if (!foundClosestHit && CharacterCollisionsSweep(
                        processor, // processor
                        tmpMovedPosition, // position
                        transientRotation, // rotation
                        remainingMovementDirection, // direction
                        remainingMovementMagnitude + Constants.CollisionOffset, // distance
                        out RaycastHit closestSweepHit, // closest hit
                        ref internalCharacterHits,
                        characterBody) // all hits 
                    > 0)
                {
                    closestSweepHitNormal = closestSweepHit.normal;
                    closestSweepHitDistance = closestSweepHit.distance;
                    closestSweepHitCollider = closestSweepHit.collider;
                    closestSweepHitPoint = closestSweepHit.point;

                    foundClosestHit = true;
                }

                if (foundClosestHit)
                {
                    // Calculate movement from this iteration
                    Vector3 sweepMovement = (remainingMovementDirection * (Mathf.Max(0f, closestSweepHitDistance - Constants.CollisionOffset)));
                    tmpMovedPosition += sweepMovement;
                    remainingMovementMagnitude -= sweepMovement.magnitude;

                    // Evaluate if hit is stable
                    HitStabilityReport moveHitStabilityReport = new HitStabilityReport();
                    processor.EvaluateHitStability(closestSweepHitCollider, closestSweepHitNormal, closestSweepHitPoint, tmpMovedPosition, transientRotation, transientVelocity, ref moveHitStabilityReport);

                    // Handle stepping up steps points higher than bottom capsule radius
                    bool foundValidStepHit = false;
                    if (true && characterBody.StepHandling && moveHitStabilityReport.ValidStepDetected) // _solveGrounding
                    {
                        float obstructionCorrelation = Mathf.Abs(Vector3.Dot(closestSweepHitNormal, groundingUp));
                        if (obstructionCorrelation <= Constants.CorrelationForVerticalObstruction)
                        {
                            Vector3 stepForwardDirection = Vector3.ProjectOnPlane(-closestSweepHitNormal, groundingUp).normalized;
                            Vector3 stepCastStartPoint = (tmpMovedPosition + (stepForwardDirection * Constants.SteppingForwardDistance)) +
                                (groundingUp * characterBody.MaxStepHeight);

                            // Cast downward from the top of the stepping height
                            int nbStepHits = CharacterCollisionsSweep(
                                                processor,
                                                stepCastStartPoint, // position
                                                transientRotation, // rotation
                                                -groundingUp, // direction
                                                characterBody.MaxStepHeight, // distance
                                                out RaycastHit closestStepHit, // closest hit
                                                ref internalCharacterHits,
                                                characterBody,
                                                0f,
                                                true); // all hits 

                            // Check for hit corresponding to stepped collider
                            for (int i = 0; i < nbStepHits; i++)
                            {
                                if (internalCharacterHits[i].collider == moveHitStabilityReport.SteppedCollider)
                                {
                                    Vector3 endStepPosition = stepCastStartPoint + (-groundingUp * (internalCharacterHits[i].distance - Constants.CollisionOffset));
                                    tmpMovedPosition = endStepPosition;
                                    foundValidStepHit = true;

                                    // Project velocity on ground normal at step 
                                    transientVelocity = Vector3.ProjectOnPlane(transientVelocity, groundingUp);
                                    remainingMovementDirection = transientVelocity.normalized;

                                    break;
                                }
                            }
                        }
                    }

                    // Handle movement solving
                    if (!foundValidStepHit)
                    {
                        Vector3 obstructionNormal = GetObstructionNormal(characterBody, groundingUp, closestSweepHitNormal, moveHitStabilityReport.IsStable);

                        bool stableOnHit = moveHitStabilityReport.IsStable && !characterBody.WillUnground();
                        Vector3 velocityBeforeProj = transientVelocity;

                        // Project velocity for next iteration
                        InternalHandleVelocityProjection(
                            stableOnHit,
                            closestSweepHitNormal,
                            obstructionNormal,
                            originalVelocityDirection,
                            ref sweepState,
                            previousHitIsStable,
                            previousVelocity,
                            previousObstructionNormal,
                            ref transientVelocity,
                            ref remainingMovementMagnitude,
                            ref remainingMovementDirection,
                            ref characterBody,
                            groundingUp);

                        previousHitIsStable = stableOnHit;
                        previousVelocity = velocityBeforeProj;
                        previousObstructionNormal = obstructionNormal;
                    }
                }
                // If we hit nothing...
                else
                {
                    hitSomethingThisSweepIteration = false;
                }

                // Safety for exceeding max sweeps allowed
                sweepsMade++;
                if (sweepsMade > characterBody.MaxMovementIterations)
                {
                    if (characterBody.KillRemainingMovementWhenExceedMaxMovementIterations)
                    {
                        remainingMovementMagnitude = 0f;
                    }

                    if (characterBody.KillVelocityWhenExceedMaxMovementIterations)
                    {
                        transientVelocity = Vector3.zero;
                    }
                    wasCompleted = false;
                }
            }

            // Move position for the remainder of the movement
            tmpMovedPosition += (remainingMovementDirection * remainingMovementMagnitude);
            transientPosition = tmpMovedPosition;

            return wasCompleted;
        }

        /// <summary>
        /// Processes movement projection upon detecting a hit
        /// </summary>
        private static void InternalHandleVelocityProjection(bool stableOnHit, Vector3 hitNormal, Vector3 obstructionNormal, Vector3 originalDirection,
            ref MovementSweepState sweepState, bool previousHitIsStable, Vector3 previousVelocity, Vector3 previousObstructionNormal,
            ref Vector3 transientVelocity, ref float remainingMovementMagnitude, ref Vector3 remainingMovementDirection, ref KinematicCharacterBody characterBody, Vector3 characterUp)
        {
            if (transientVelocity.sqrMagnitude <= 0f)
            {
                return;
            }

            Vector3 velocityBeforeProjection = transientVelocity;

            if (stableOnHit)
            {
                characterBody.LastMovementIterationFoundAnyGround = true;
                HandleVelocityProjection(ref transientVelocity, obstructionNormal, stableOnHit, characterBody, characterUp);
            }
            else
            {
                // Handle projection
                if (sweepState == MovementSweepState.Initial)
                {
                    HandleVelocityProjection(ref transientVelocity, obstructionNormal, stableOnHit, characterBody, characterUp);
                    sweepState = MovementSweepState.AfterFirstHit;
                }
                // Blocking crease handling
                else if (sweepState == MovementSweepState.AfterFirstHit)
                {
                    EvaluateCrease(
                        transientVelocity,
                        previousVelocity,
                        obstructionNormal,
                        previousObstructionNormal,
                        stableOnHit,
                        previousHitIsStable,
                        characterBody.GroundingStatus.IsStableOnGround && !characterBody.WillUnground(),
                        out bool foundCrease,
                        out Vector3 creaseDirection);

                    if (foundCrease)
                    {
                        if (characterBody.GroundingStatus.IsStableOnGround && !characterBody.WillUnground())
                        {
                            transientVelocity = Vector3.zero;
                            sweepState = MovementSweepState.FoundBlockingCorner;
                        }
                        else
                        {
                            transientVelocity = Vector3.Project(transientVelocity, creaseDirection);
                            sweepState = MovementSweepState.FoundBlockingCrease;
                        }
                    }
                    else
                    {
                        HandleVelocityProjection(ref transientVelocity, obstructionNormal, stableOnHit, characterBody, characterUp);
                    }
                }
                // Blocking corner handling
                else if (sweepState == MovementSweepState.FoundBlockingCrease)
                {
                    transientVelocity = Vector3.zero;
                    sweepState = MovementSweepState.FoundBlockingCorner;
                }
            }

            float newVelocityFactor = transientVelocity.magnitude / velocityBeforeProjection.magnitude;
            remainingMovementMagnitude *= newVelocityFactor;
            remainingMovementDirection = transientVelocity.normalized;
        }

        private static void EvaluateCrease(
            Vector3 currentCharacterVelocity,
            Vector3 previousCharacterVelocity,
            Vector3 currentHitNormal,
            Vector3 previousHitNormal,
            bool currentHitIsStable,
            bool previousHitIsStable,
            bool characterIsStable,
            out bool isValidCrease,
            out Vector3 creaseDirection)
        {
            isValidCrease = false;
            creaseDirection = default;

            if (!characterIsStable || !currentHitIsStable || !previousHitIsStable)
            {
                Vector3 tmpBlockingCreaseDirection = Vector3.Cross(currentHitNormal, previousHitNormal).normalized;
                float dotPlanes = Vector3.Dot(currentHitNormal, previousHitNormal);
                bool isVelocityConstrainedByCrease = false;

                // Avoid calculations if the two planes are the same
                if (dotPlanes < 0.999f)
                {
                    // TODO: can this whole part be made simpler? (with 2d projections, etc)
                    Vector3 normalAOnCreasePlane = Vector3.ProjectOnPlane(currentHitNormal, tmpBlockingCreaseDirection).normalized;
                    Vector3 normalBOnCreasePlane = Vector3.ProjectOnPlane(previousHitNormal, tmpBlockingCreaseDirection).normalized;
                    float dotPlanesOnCreasePlane = Vector3.Dot(normalAOnCreasePlane, normalBOnCreasePlane);

                    Vector3 enteringVelocityDirectionOnCreasePlane = Vector3.ProjectOnPlane(previousCharacterVelocity, tmpBlockingCreaseDirection).normalized;

                    if (dotPlanesOnCreasePlane <= (Vector3.Dot(-enteringVelocityDirectionOnCreasePlane, normalAOnCreasePlane) + 0.001f) &&
                        dotPlanesOnCreasePlane <= (Vector3.Dot(-enteringVelocityDirectionOnCreasePlane, normalBOnCreasePlane) + 0.001f))
                    {
                        isVelocityConstrainedByCrease = true;
                    }
                }

                if (isVelocityConstrainedByCrease)
                {
                    // Flip crease direction to make it representative of the real direction our velocity would be projected to
                    if (Vector3.Dot(tmpBlockingCreaseDirection, currentCharacterVelocity) < 0f)
                    {
                        tmpBlockingCreaseDirection = -tmpBlockingCreaseDirection;
                    }

                    isValidCrease = true;
                    creaseDirection = tmpBlockingCreaseDirection;
                }
            }
        }

        /// <summary>
        /// Allows you to override the way velocity is projected on an obstruction
        /// </summary>
        public static void HandleVelocityProjection(ref Vector3 velocity, Vector3 obstructionNormal, bool stableOnHit, KinematicCharacterBody characterBody, Vector3 characterUp)
        {
            if (characterBody.GroundingStatus.IsStableOnGround && !characterBody.WillUnground())
            {
                // On stable slopes, simply reorient the movement without any loss
                if (stableOnHit)
                {
                    velocity = GetDirectionTangentToSurface(velocity, obstructionNormal, characterUp) * velocity.magnitude;
                }
                // On blocking hits, project the movement on the obstruction while following the grounding plane
                else
                {
                    Vector3 obstructionRightAlongGround = Vector3.Cross(obstructionNormal, characterBody.GroundingStatus.GroundNormal).normalized;
                    Vector3 obstructionUpAlongGround = Vector3.Cross(obstructionRightAlongGround, obstructionNormal).normalized;
                    velocity = GetDirectionTangentToSurface(velocity, obstructionUpAlongGround, characterUp) * velocity.magnitude;
                    velocity = Vector3.ProjectOnPlane(velocity, obstructionNormal);
                }
            }
            else
            {
                if (stableOnHit)
                {
                    // Handle stable landing
                    velocity = Vector3.ProjectOnPlane(velocity, characterUp);
                    velocity = GetDirectionTangentToSurface(velocity, obstructionNormal, characterUp) * velocity.magnitude;
                }
                // Handle generic obstruction
                else
                {
                    velocity = Vector3.ProjectOnPlane(velocity, obstructionNormal);
                }
            }
        }


        /// <summary>
        /// Determines if the motor is considered stable on a given hit
        /// </summary>
        public static void EvaluateHitStability<T>(ref T processor, ref RaycastHit[] internalCharacterHits, Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, Vector3 withCharacterVelocity, ref HitStabilityReport stabilityReport, KinematicCharacterBody characterBody, Vector3 groundingUp) where T : struct, IKinematicCharacterProcessor
        {
            if (!characterBody.SolveGrounding)
            {
                stabilityReport.IsStable = false;
                return;
            }

            Vector3 atCharacterUp = atCharacterRotation * Vector3.up;
            Vector3 innerHitDirection = Vector3.ProjectOnPlane(hitNormal, atCharacterUp).normalized;

            stabilityReport.IsStable = IsStableOnNormal(hitNormal, atCharacterUp, characterBody.MaxStableSlopeAngle);

            stabilityReport.FoundInnerNormal = false;
            stabilityReport.FoundOuterNormal = false;
            stabilityReport.InnerNormal = hitNormal;
            stabilityReport.OuterNormal = hitNormal;

            // Ledge handling
            if (characterBody.LedgeAndDenivelationHandling)
            {
                float ledgeCheckHeight = Constants.MinDistanceForLedge;
                if (characterBody.StepHandling)
                {
                    ledgeCheckHeight = characterBody.MaxStepHeight;
                }

                bool isStableLedgeInner = false;
                bool isStableLedgeOuter = false;

                if (CharacterCollisionsRaycast(
                        processor,
                        hitPoint + (atCharacterUp * Constants.SecondaryProbesVertical) + (innerHitDirection * Constants.SecondaryProbesHorizontal),
                        -atCharacterUp,
                        ledgeCheckHeight + Constants.SecondaryProbesVertical,
                        out RaycastHit innerLedgeHit,
                        internalCharacterHits,
                        characterBody.StableGroundLayers,
                        characterBody.Capsule) > 0)
                {
                    Vector3 innerLedgeNormal = innerLedgeHit.normal;
                    stabilityReport.InnerNormal = innerLedgeNormal;
                    stabilityReport.FoundInnerNormal = true;
                    isStableLedgeInner = IsStableOnNormal(innerLedgeNormal, atCharacterUp, characterBody.MaxStableSlopeAngle);
                }

                if (CharacterCollisionsRaycast(
                        processor,
                        hitPoint + (atCharacterUp * Constants.SecondaryProbesVertical) + (-innerHitDirection * Constants.SecondaryProbesHorizontal),
                        -atCharacterUp,
                        ledgeCheckHeight + Constants.SecondaryProbesVertical,
                        out RaycastHit outerLedgeHit,
                        internalCharacterHits,
                        characterBody.StableGroundLayers,
                        characterBody.Capsule) > 0)
                {
                    Vector3 outerLedgeNormal = outerLedgeHit.normal;
                    stabilityReport.OuterNormal = outerLedgeNormal;
                    stabilityReport.FoundOuterNormal = true;
                    isStableLedgeOuter = IsStableOnNormal(outerLedgeNormal, atCharacterUp, characterBody.MaxStableSlopeAngle);
                }

                stabilityReport.LedgeDetected = (isStableLedgeInner != isStableLedgeOuter);
                if (stabilityReport.LedgeDetected)
                {
                    stabilityReport.IsOnEmptySideOfLedge = isStableLedgeOuter && !isStableLedgeInner;
                    stabilityReport.LedgeGroundNormal = isStableLedgeOuter ? stabilityReport.OuterNormal : stabilityReport.InnerNormal;
                    stabilityReport.LedgeRightDirection = Vector3.Cross(hitNormal, stabilityReport.LedgeGroundNormal).normalized;
                    stabilityReport.LedgeFacingDirection = Vector3.ProjectOnPlane(Vector3.Cross(stabilityReport.LedgeGroundNormal, stabilityReport.LedgeRightDirection), groundingUp).normalized;
                    stabilityReport.DistanceFromLedge = Vector3.ProjectOnPlane((hitPoint - (atCharacterPosition + (atCharacterRotation * GetCapsuleBottom(characterBody.Capsule)))), atCharacterUp).magnitude;
                    stabilityReport.IsMovingTowardsEmptySideOfLedge = Vector3.Dot(withCharacterVelocity.normalized, stabilityReport.LedgeFacingDirection) > 0f;
                }

                if (stabilityReport.IsStable)
                {
                    stabilityReport.IsStable = IsStableWithSpecialCases(ref stabilityReport, withCharacterVelocity, characterBody);
                }
            }

            // Step handling 
            if (characterBody.StepHandling && !stabilityReport.IsStable)
            {
                // Stepping not supported on dynamic rigidbodies
                Rigidbody hitRigidbody = hitCollider.attachedRigidbody;
                if (!(hitRigidbody && !hitRigidbody.isKinematic))
                {
                    DetectSteps(processor, atCharacterPosition, atCharacterRotation, hitPoint, innerHitDirection, ref stabilityReport, characterBody, ref internalCharacterHits);

                    if (stabilityReport.ValidStepDetected)
                    {
                        stabilityReport.IsStable = true;
                    }
                }
            }
        }

        private static void DetectSteps(IKinematicCharacterProcessor processor, Vector3 characterPosition, Quaternion characterRotation, Vector3 hitPoint, Vector3 innerHitDirection, ref HitStabilityReport stabilityReport, KinematicCharacterBody characterBody, ref RaycastHit[] internalCharacterHits)
        {
            int nbStepHits = 0;
            Collider tmpCollider;
            RaycastHit outerStepHit;
            Vector3 characterUp = characterRotation * Vector3.up;
            Vector3 verticalCharToHit = Vector3.Project((hitPoint - characterPosition), characterUp);
            Vector3 horizontalCharToHitDirection = Vector3.ProjectOnPlane((hitPoint - characterPosition), characterUp).normalized;
            Vector3 stepCheckStartPos = (hitPoint - verticalCharToHit) + (characterUp * characterBody.MaxStepHeight) + (horizontalCharToHitDirection * Constants.CollisionOffset * 3f);

            // Do outer step check with capsule cast on hit point
            nbStepHits = CharacterCollisionsSweep(
                            processor,
                           stepCheckStartPos,
                           characterRotation,
                           -characterUp,
                           characterBody.MaxStepHeight + Constants.CollisionOffset,
                           out outerStepHit,
                           ref internalCharacterHits,
                           characterBody,
                           0f,
                           true);

            // Check for overlaps and obstructions at the hit position
            if (processor.CheckStepValidity(nbStepHits, characterPosition, characterRotation, innerHitDirection, stepCheckStartPos, out tmpCollider))
            {
                stabilityReport.ValidStepDetected = true;
                stabilityReport.SteppedCollider = tmpCollider;
            }

            if (characterBody.StepHandling && !stabilityReport.ValidStepDetected)
            {
                // Do min reach step check with capsule cast on hit point
                stepCheckStartPos = characterPosition + (characterUp * characterBody.MaxStepHeight) + (-innerHitDirection * characterBody.ExtraStepChecksDistance);
                nbStepHits = CharacterCollisionsSweep(
                                processor,
                                stepCheckStartPos,
                                characterRotation,
                                -characterUp,
                                characterBody.MaxStepHeight - Constants.CollisionOffset,
                                out outerStepHit,
                                ref internalCharacterHits,
                                characterBody,
                                0f,
                                true);

                // Check for overlaps and obstructions at the hit position
                if (processor.CheckStepValidity(nbStepHits, characterPosition, characterRotation, innerHitDirection, stepCheckStartPos, out tmpCollider))
                {
                    stabilityReport.ValidStepDetected = true;
                    stabilityReport.SteppedCollider = tmpCollider;
                }
            }
        }

        public static bool CheckStepValidity(IKinematicCharacterProcessor processor, int nbStepHits, Vector3 characterPosition, Quaternion characterRotation, Vector3 innerHitDirection, Vector3 stepCheckStartPos, out Collider hitCollider, KinematicCharacterBody characterBody, ref RaycastHit[] internalCharacterHits, ref Collider[] internalProbedColliders, Vector3 groundingUp)
        {
            hitCollider = null;

            // Find the farthest valid hit for stepping
            bool foundValidStepPosition = false;

            while (nbStepHits > 0 && !foundValidStepPosition)
            {
                // Get farthest hit among the remaining hits
                RaycastHit farthestHit = new RaycastHit();
                float farthestDistance = 0f;
                int farthestIndex = 0;
                for (int i = 0; i < nbStepHits; i++)
                {
                    float hitDistance = internalCharacterHits[i].distance;
                    if (hitDistance > farthestDistance)
                    {
                        farthestDistance = hitDistance;
                        farthestHit = internalCharacterHits[i];
                        farthestIndex = i;
                    }
                }

                Vector3 characterPositionAtHit = stepCheckStartPos + (-groundingUp * (farthestHit.distance - Constants.CollisionOffset));

                int atStepOverlaps = CharacterCollisionsOverlap(processor, characterBody.Capsule, characterPositionAtHit, characterRotation, ref internalProbedColliders);
                if (atStepOverlaps <= 0)
                {
                    // Check for outer hit slope normal stability at the step position
                    if (CharacterCollisionsRaycast(
                            processor,
                            farthestHit.point + (groundingUp * Constants.SecondaryProbesVertical) + (-innerHitDirection * Constants.SecondaryProbesHorizontal),
                            -groundingUp,
                            characterBody.MaxStepHeight + Constants.SecondaryProbesVertical,
                            out RaycastHit outerSlopeHit,
                            internalCharacterHits,
                            characterBody.StableGroundLayers,
                            characterBody.Capsule,
                            true) > 0)
                    {
                        if (IsStableOnNormal(outerSlopeHit.normal, groundingUp, characterBody.MaxStableSlopeAngle))
                        {
                            // Cast upward to detect any obstructions to moving there
                            if (CharacterCollisionsSweep(
                                                processor,
                                                characterPosition, // position
                                                characterRotation, // rotation
                                                groundingUp, // direction
                                                characterBody.MaxStepHeight - farthestHit.distance, // distance
                                                out RaycastHit tmpUpObstructionHit, // closest hit
                                                ref internalCharacterHits,
                                                characterBody) // all hits
                                    <= 0)
                            {
                                // Do inner step check...
                                bool innerStepValid = false;
                                RaycastHit innerStepHit;

                                if (true) // AllowSteppingWithoutStableGrounding
                                {
                                    innerStepValid = true;
                                }
                                else
                                {
                                    /*
                                    // At the capsule center at the step height
                                    if (CharacterCollisionsRaycast(
                                            characterPosition + Vector3.Project((characterPositionAtHit - characterPosition), groundingUp),
                                            -groundingUp,
                                            characterBody.MaxStepHeight,
                                            out innerStepHit,
                                            internalCharacterHits,
                                            characterBody.StableGroundLayers,
                                            characterBody.Capsule,
                                            true) > 0)
                                    {
                                        if (IsStableOnNormal(innerStepHit.normal, groundingUp, characterBody.MaxStableSlopeAngle))
                                        {
                                            innerStepValid = true;
                                        }
                                    }*/
                                }

                                if (!innerStepValid)
                                {
                                    // At inner step of the step point
                                    if (CharacterCollisionsRaycast(
                                            processor,
                                            farthestHit.point + (innerHitDirection * Constants.SecondaryProbesHorizontal),
                                            -groundingUp,
                                            characterBody.MaxStepHeight,
                                            out innerStepHit,
                                            internalCharacterHits,
                                            characterBody.StableGroundLayers,
                                            characterBody.Capsule,
                                            true) > 0)
                                    {
                                        if (IsStableOnNormal(innerStepHit.normal, groundingUp, characterBody.MaxStableSlopeAngle))
                                        {
                                            innerStepValid = true;
                                        }
                                    }
                                }

                                // Final validation of step
                                if (innerStepValid)
                                {
                                    hitCollider = farthestHit.collider;
                                    foundValidStepPosition = true;
                                    return true;
                                }
                            }
                        }
                    }
                }

                // Discard hit if not valid step
                if (!foundValidStepPosition)
                {
                    nbStepHits--;
                    if (farthestIndex < nbStepHits)
                    {
                        internalCharacterHits[farthestIndex] = internalCharacterHits[nbStepHits];
                    }
                }
            }

            return false;
        }

        // <summary>
        /// Sweeps the capsule's volume to detect collision hits
        /// </summary>
        /// <returns> Returns the number of hits </returns>
        public static int CharacterCollisionsSweep(IKinematicCharacterProcessor processor, Vector3 position, Quaternion rotation, Vector3 direction, float distance, out RaycastHit closestHit, ref RaycastHit[] hits, KinematicCharacterBody characterBody, float inflate = 0f, bool acceptOnlyStableGroundLayer = false)
        {
            LayerMask CollidableLayers = -1;
            int queryLayers = CollidableLayers;

            if (acceptOnlyStableGroundLayer)
            {
                queryLayers = CollidableLayers & characterBody.StableGroundLayers;
            }

            Vector3 bottom = position + (rotation * GetCapsuleBottomHemi(characterBody.Capsule)) - (direction * Constants.SweepProbingBackstepDistance);
            Vector3 top = position + (rotation * GetCapsuleTopHemi(characterBody.Capsule)) - (direction * Constants.SweepProbingBackstepDistance);
            if (inflate != 0f)
            {
                bottom += (rotation * Vector3.down * inflate);
                top += (rotation * Vector3.up * inflate);
            }

            // Capsule cast
            int nbHits = 0;
            int nbUnfilteredHits = UnityEngine.Physics.CapsuleCastNonAlloc(
                    bottom,
                    top,
                    characterBody.Capsule.radius + inflate,
                    direction,
                    hits,
                    distance + Constants.SweepProbingBackstepDistance,
                    queryLayers,
                    QueryTriggerInteraction.Ignore);

            // Hits filter
            closestHit = new RaycastHit();
            float closestDistance = Mathf.Infinity;
            nbHits = nbUnfilteredHits;
            for (int i = nbUnfilteredHits - 1; i >= 0; i--)
            {
                hits[i].distance -= Constants.SweepProbingBackstepDistance;

                RaycastHit hit = hits[i];
                float hitDistance = hit.distance;

                // Filter out the invalid hits 
                if (hitDistance <= 0f || !CheckIfColliderValidForCollisions(processor, characterBody.Capsule, hit.collider))
                {
                    nbHits--;
                    if (i < nbHits)
                    {
                        hits[i] = hits[nbHits];
                    }
                }
                else
                {
                    // Remember closest valid hit
                    if (hitDistance < closestDistance)
                    {
                        closestHit = hit;
                        closestDistance = hitDistance;
                    }
                }
            }

            return nbHits;
        }

        /// <summary>
        /// Raycasts to detect collision hits
        /// </summary>
        /// <returns> Returns the number of hits </returns>
        public static int CharacterCollisionsRaycast(IKinematicCharacterProcessor processor, Vector3 position, Vector3 direction, float distance, out RaycastHit closestHit, RaycastHit[] hits, LayerMask StableGroundLayers, CapsuleCollider capsule, bool acceptOnlyStableGroundLayer = false)
        {
            LayerMask CollidableLayers = -1; //TODO

            int queryLayers = CollidableLayers;
            if (acceptOnlyStableGroundLayer)
            {
                queryLayers = CollidableLayers & StableGroundLayers;
            }

            // Raycast
            int nbHits = 0;
            int nbUnfilteredHits = UnityEngine.Physics.RaycastNonAlloc(
                position,
                direction,
                hits,
                distance,
                queryLayers,
                QueryTriggerInteraction.Ignore);

            // Hits filter
            closestHit = new RaycastHit();
            float closestDistance = Mathf.Infinity;
            nbHits = nbUnfilteredHits;
            for (int i = nbUnfilteredHits - 1; i >= 0; i--)
            {
                RaycastHit hit = hits[i];
                float hitDistance = hit.distance;

                // Filter out the invalid hits
                if (hitDistance <= 0f ||
                    !CheckIfColliderValidForCollisions(processor, capsule, hit.collider))
                {
                    nbHits--;
                    if (i < nbHits)
                    {
                        hits[i] = hits[nbHits];
                    }
                }
                else
                {
                    // Remember closest valid hit
                    if (hitDistance < closestDistance)
                    {
                        closestHit = hit;
                        closestDistance = hitDistance;
                    }
                }
            }

            return nbHits;
        }

        /// <summary>
        /// Casts the character volume in the character's downward direction to detect ground
        /// </summary>
        /// <returns> Returns the number of hits </returns>
        public static bool CharacterGroundSweep(ref RaycastHit[] internalCharacterHits, Vector3 position, Quaternion rotation, Vector3 direction, float distance, out RaycastHit closestHit, CapsuleCollider Capsule, IKinematicCharacterProcessor processor, LayerMask StableGroundLayers)
        {
            closestHit = new RaycastHit();
            LayerMask CollidableLayers = -1; // TODO

            // Capsule cast
            int nbUnfilteredHits = UnityEngine.Physics.CapsuleCastNonAlloc(
                position + (rotation * GetCapsuleBottomHemi(Capsule)) - (direction * Constants.GroundProbingBackstepDistance),
                position + (rotation * GetCapsuleTopHemi(Capsule)) - (direction * Constants.GroundProbingBackstepDistance),
                Capsule.radius,
                direction,
                internalCharacterHits,
                distance + Constants.GroundProbingBackstepDistance,
                CollidableLayers & StableGroundLayers,
                QueryTriggerInteraction.Ignore);

            // Hits filter
            bool foundValidHit = false;
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < nbUnfilteredHits; i++)
            {
                RaycastHit hit = internalCharacterHits[i];
                float hitDistance = hit.distance;

                // Find the closest valid hit
                if (hitDistance > 0f && CheckIfColliderValidForCollisions(processor, Capsule, hit.collider))
                {
                    if (hitDistance < closestDistance)
                    {
                        closestHit = hit;
                        closestHit.distance -= Constants.GroundProbingBackstepDistance;
                        closestDistance = hitDistance;

                        foundValidHit = true;
                    }
                }
            }

            return foundValidHit;
        }

        /// <summary>
        /// Determines if motor can be considered stable on given slope normal
        /// </summary>
        private static bool IsStableWithSpecialCases(ref HitStabilityReport stabilityReport, Vector3 velocity, KinematicCharacterBody characterBody)
        {
            if (characterBody.LedgeAndDenivelationHandling)
            {
                if (stabilityReport.LedgeDetected)
                {
                    if (stabilityReport.IsMovingTowardsEmptySideOfLedge)
                    {
                        // Max snap vel
                        Vector3 velocityOnLedgeNormal = Vector3.Project(velocity, stabilityReport.LedgeFacingDirection);
                        if (velocityOnLedgeNormal.magnitude >= characterBody.MaxVelocityForLedgeSnap)
                        {
                            return false;
                        }
                    }

                    // Distance from ledge
                    if (stabilityReport.IsOnEmptySideOfLedge && stabilityReport.DistanceFromLedge > characterBody.MaxStableDistanceFromLedge)
                    {
                        return false;
                    }
                }

                // "Launching" off of slopes of a certain denivelation angle
                if (characterBody.LastGroundingStatus.FoundAnyGround && stabilityReport.InnerNormal.sqrMagnitude != 0f && stabilityReport.OuterNormal.sqrMagnitude != 0f)
                {
                    float denivelationAngle = Vector3.Angle(stabilityReport.InnerNormal, stabilityReport.OuterNormal);
                    if (denivelationAngle > characterBody.MaxStableDenivelationAngle)
                    {
                        return false;
                    }
                    else
                    {
                        denivelationAngle = Vector3.Angle(characterBody.LastGroundingStatus.InnerGroundNormal, stabilityReport.OuterNormal);
                        if (denivelationAngle > characterBody.MaxStableDenivelationAngle)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // <summary>
        /// Gets the effective normal for movement obstruction depending on current grounding status
        /// </summary>
        private static Vector3 GetObstructionNormal(KinematicCharacterBody characterBody, Vector3 groundingUp, Vector3 hitNormal, bool stableOnHit)
        {
            // Find hit/obstruction/offset normal
            Vector3 obstructionNormal = hitNormal;
            if (characterBody.GroundingStatus.IsStableOnGround && !characterBody.WillUnground() && !stableOnHit)
            {
                Vector3 obstructionLeftAlongGround = Vector3.Cross(characterBody.GroundingStatus.GroundNormal, obstructionNormal).normalized;
                obstructionNormal = Vector3.Cross(obstructionLeftAlongGround, groundingUp).normalized;
            }

            // Catch cases where cross product between parallel normals returned 0
            if (obstructionNormal.sqrMagnitude == 0f)
            {
                obstructionNormal = hitNormal;
            }

            return obstructionNormal;
        }

        /// <summary>
        /// Detect if the character capsule is overlapping with anything collidable
        /// </summary>
        /// <returns> Returns number of overlaps </returns>
        /// <summary>
        /// Detect if the character capsule is overlapping with anything collidable
        /// </summary>
        /// <returns> Returns number of overlaps </returns> 
        public static int CharacterCollisionsOverlap(IKinematicCharacterProcessor processor, CapsuleCollider Capsule, Vector3 position, Quaternion rotation, ref Collider[] overlappedColliders, float inflate = 0f, bool acceptOnlyStableGroundLayer = false)
        {
            int queryLayers = -1;
            if (acceptOnlyStableGroundLayer)
            {
                //queryLayers = CollidableLayers & StableGroundLayers;
            }

            Vector3 bottom = position + (rotation * GetCapsuleBottomHemi(Capsule));
            Vector3 top = position + (rotation * GetCapsuleTopHemi(Capsule));
            if (inflate != 0f)
            {
                bottom += (rotation * Vector3.down * inflate);
                top += (rotation * Vector3.up * inflate);
            }

            int nbHits = 0;
            int nbUnfilteredHits = UnityEngine.Physics.OverlapCapsuleNonAlloc(
                        bottom,
                        top,
                        Capsule.radius + inflate,
                        overlappedColliders,
                        queryLayers,
                        QueryTriggerInteraction.Ignore);

            // Filter out invalid colliders
            nbHits = nbUnfilteredHits;
            for (int i = nbUnfilteredHits - 1; i >= 0; i--)
            {
                if (!CheckIfColliderValidForCollisions(processor, Capsule, overlappedColliders[i]))
                {
                    nbHits--;
                    if (i < nbHits)
                    {
                        overlappedColliders[i] = overlappedColliders[nbHits];
                    }
                }
            }

            return nbHits;
        }

        /// <summary>
        /// Determines if the input collider is valid for collision processing
        /// </summary>
        /// <returns> Returns true if the collider is valid </returns>
        private static bool CheckIfColliderValidForCollisions(IKinematicCharacterProcessor processor, CapsuleCollider capsule, Collider coll)
        {
            // Ignore self
            if (coll == capsule)
            {
                return false;
            }

            if (processor.IsColliderValidForCollisions(coll))
            {
                return false;
            }

            if (processor.IsInIgnoreLayer(coll))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the direction adjusted to be tangent to a specified surface normal relatively to the character's up direction.
        /// Useful for reorienting a direction on a slope without any lateral deviation in trajectory
        /// </summary>
        public static Vector3 GetDirectionTangentToSurface(Vector3 direction, Vector3 surfaceNormal, Vector3 groundingUp)
        {
            Vector3 directionRight = Vector3.Cross(direction, groundingUp);
            return Vector3.Cross(surfaceNormal, directionRight).normalized;
        }

        /// <summary>
        /// Determines if motor can be considered stable on given slope normal
        /// </summary>
        private static bool IsStableOnNormal(Vector3 normal, Vector3 characterUp, float maxStableSlopeAngle)
        {
            return Vector3.Angle(characterUp, normal) <= maxStableSlopeAngle;
        }

        public static Vector3 GetCapsuleCenter(CapsuleCollider Capsule) { return Capsule.center; }
        public static Vector3 GetCapsuleBottom(CapsuleCollider Capsule) { return Capsule.center + (-Vector3.up * (Capsule.height * 0.5f)); }
        public static Vector3 GetCapsuleTop(CapsuleCollider Capsule) { return Capsule.center + (Vector3.up * (Capsule.height * 0.5f)); }
        public static Vector3 GetCapsuleBottomHemi(CapsuleCollider Capsule) { return Capsule.center + (-Vector3.up * (Capsule.height * 0.5f)) + (Vector3.up * Capsule.radius); }
        public static Vector3 GetCapsuleTopHemi(CapsuleCollider Capsule) { return Capsule.center + (Vector3.up * (Capsule.height * 0.5f)) + (-Vector3.up * Capsule.radius); }
    }
}
