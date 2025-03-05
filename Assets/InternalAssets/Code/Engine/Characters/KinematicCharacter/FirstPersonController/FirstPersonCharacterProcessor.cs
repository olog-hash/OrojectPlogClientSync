using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using ProjectOlog.Code.Engine.Characters.PlayerInput.FirstPerson;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController
{
    public struct FirstPersonCharacterProcessor : IKinematicCharacterProcessor
    {
        public float DeltaTime;

        public Entity Entity;
        public Vector3 Translation;
        public Quaternion Rotation;
        public KinematicCharacterBody CharacterBody;
        public FirstPersonCharacter FirstPersonCharacter;
        public FirstPersonInputs FirstPersonInputs;
        public TransitionStateRequestComponent TransitionStateRequestComponent;
        public FirstCharacterStateMachine FirstCharacterStateMachine;
        public CharacterBodyLogger CharacterBodyLogger;

        public RaycastHit[] InternalCharacterHits;
        public Collider[] InternalProbedColliders;
        public OverlapResult[] Overlaps;
        public int OverlapsCount;

        #region Processor Getters
        public float GetDeltaTime => DeltaTime;
        #endregion

        #region Processor Callbacks
        public bool CharacterGroundSweep(
            Vector3 groundSweepPosition, // position
            Quaternion atRotation, // rotation
            Vector3 groundSweepDirection, // direction
            float groundProbeDistanceRemaining, // distance
            out RaycastHit groundSweepHit)
        {
            return KinematicCharacterUtilities.CharacterGroundSweep(
                ref InternalCharacterHits,
                groundSweepPosition,
                atRotation,
                groundSweepDirection,
                groundProbeDistanceRemaining,
                out groundSweepHit,
                CharacterBody.Capsule,
                this,
                CharacterBody.StableGroundLayers);
        }

        public void EvaluateHitStability(
            Collider hitCollider,
            Vector3 hitNormal,
            Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation,
            Vector3 withCharacterVelocity,
            ref HitStabilityReport stabilityReport)
        {
            KinematicCharacterUtilities.EvaluateHitStability(ref this, ref InternalCharacterHits, hitCollider, hitNormal, hitPoint, atCharacterPosition, atCharacterRotation, withCharacterVelocity, ref stabilityReport, CharacterBody, FirstPersonCharacter.GroundingUp);
        }

        public bool CheckStepValidity(
            int nbStepHits,
            Vector3 characterPosition,
            Quaternion characterRotation,
            Vector3 innerHitDirection,
            Vector3 stepCheckStartPos,
            out Collider hitCollider)
        {
            return KinematicCharacterUtilities.CheckStepValidity(this, nbStepHits, characterPosition, characterRotation, innerHitDirection, stepCheckStartPos, out hitCollider, CharacterBody, ref InternalCharacterHits, ref InternalProbedColliders, FirstPersonCharacter.GroundingUp);
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return CharacterBody.IgnoredColliders.Contains(coll);
        }

        public bool IsInIgnoreLayer(Collider coll)
        {
            // Получаем слой коллайдера
            int colliderLayer = coll.gameObject.layer;

            // Проверяем, входит ли слой в маску
            return (CharacterBody.IgnoreLayers.value & (1 << colliderLayer)) != 0;
        }

        #endregion


        public void OnUpdate()
        {
            // Базовая инициализация
            FirstPersonCharacter.GroundingUp = Vector3.up;
            bool wasGrounded = CharacterBody.GroundingStatus.IsStableOnGround;
            Vector3 initialVelocity = CharacterBody.BaseVelocity;
            
            // Основное обновление состояния персонажа
            KinematicCharacterUtilities.InitializationUpdate(ref CharacterBody, ref InternalCharacterHits, ref InternalProbedColliders, ref Overlaps, ref OverlapsCount);
            
            FirstCharacterStateMachine.OnStateUpdate(FirstCharacterStateMachine.CurrentCharacterState, ref this);
            
            // Проверка приземления и обновление логгера
            bool isGroundedNow = CharacterBody.GroundingStatus.IsStableOnGround;
            
            CharacterBodyLogger.ViewPitchDegrees = FirstPersonCharacter.ViewPitchDegrees;
            CharacterBodyLogger.IsGrounded = wasGrounded || isGroundedNow;
            CharacterBodyLogger.PreviousFallVelocity = initialVelocity.y;
        }

        public void CharacterCollisionAndGroundingUpdate()
        {
            KinematicCharacterUtilities.SolvingCollisionOverlaps(this, ref Translation, ref InternalProbedColliders, ref Overlaps, ref OverlapsCount, FirstPersonCharacter.GroundingUp, Rotation, CharacterBody);
            KinematicCharacterUtilities.GroundingUpdate(ref this, ref Translation, ref CharacterBody, Rotation, FirstPersonCharacter.GroundingUp);
        }

        public void CharacterMovementAndFinalizationUpdate()
        {
            KinematicCharacterUtilities.MovementAndDecollisionsUpdate(ref this, ref CharacterBody, ref Translation, Rotation, FirstPersonCharacter.GroundingUp, ref InternalCharacterHits, ref InternalProbedColliders, ref Overlaps, OverlapsCount, DeltaTime);
        }

        public void TransitionToState(CharacterState newState)
        {
            FirstCharacterStateMachine.PreviousCharacterState = FirstCharacterStateMachine.CurrentCharacterState;
            FirstCharacterStateMachine.CurrentCharacterState = newState;

            FirstCharacterStateMachine.OnStateExit(FirstCharacterStateMachine.PreviousCharacterState, FirstCharacterStateMachine.CurrentCharacterState, ref this);
            FirstCharacterStateMachine.OnStateEnter(FirstCharacterStateMachine.CurrentCharacterState, FirstCharacterStateMachine.PreviousCharacterState, ref this);
        }

        public bool DetectGlobalTransitions()
        {
            if (FirstPersonInputs.NoClipRequested)
            {
                if (FirstCharacterStateMachine.CurrentCharacterState == CharacterState.FlyingNoCollisions)
                {
                    TransitionToState(CharacterState.GroundMove);
                    return true;
                }
                else
                {
                    TransitionToState(CharacterState.FlyingNoCollisions);
                    return true;
                }
            }

            if (TransitionStateRequestComponent.IsActive)
            {
                TransitionToState(TransitionStateRequestComponent.NextCharacterState);
                return true;
            }

            return false;
        }

        private Vector2 CalculateMoveVector()
        {
            Vector2 inputMoveDirection = new Vector2(FirstPersonInputs.MoveVector.x, FirstPersonInputs.MoveVector.z);

// Получаем угол поворота вокруг оси Y в радианах
            float rotationAngle = Rotation.eulerAngles.y * Mathf.Deg2Rad;

// Вычисляем синус и косинус угла
            float sin = Mathf.Sin(rotationAngle);
            float cos = Mathf.Cos(rotationAngle);

// Применяем поворот к вектору движения
            Vector2 rotatedMoveDirection = new Vector2(
                inputMoveDirection.x * cos - inputMoveDirection.y * sin,
                inputMoveDirection.x * sin + inputMoveDirection.y * cos
            );
            
            // Инвертируем X-компонент для исправления инверсии право-лево
            rotatedMoveDirection.x = -rotatedMoveDirection.x;

            return rotatedMoveDirection;
        }
    }
}
