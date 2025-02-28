using ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{

    public enum CharacterState
    {
        GroundMove,
        Crouched,
        FlyingNoCollisions,
    }

    public interface ICharacterState
    {
        void OnStateEnter(CharacterState previousState, ref FirstPersonCharacterProcessor processor);
        void OnStateExit(CharacterState nextState, ref FirstPersonCharacterProcessor processor);
        void OnStateUpdate(ref FirstPersonCharacterProcessor processor);
    }


    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct FirstPersonCharacter : IComponent
    {
        [Header("Movement")]
        public float WalkSpeed;
        public float SprintSpeed;
        public float CrouchSpeed;
        public float AlternativeModeSpeed;
        public float RotationSharpness;
        public float MovementSharpness;
        public float AirAcceleration;
        public float AirMaxSpeed;
        public float AirDrag;
        public float JumpSpeed;
        public float JumpPreGroundingGraceTime;
        public float JumpPostGroundingGraceTime;
        public float MaxFallSpeed;
        public Vector3 Gravity;

        [Header("View Limits")]
        public float MinVAngle;
        public float MaxVAngle;

        [HideInInspector] public EntityProvider CharacterViewEntity;
        [HideInInspector] public float ViewYawDegrees;
        [HideInInspector] public float ViewPitchDegrees;
        [HideInInspector] public float CameraPointHeight;
        [HideInInspector] public Vector3 GroundingUp;
        [HideInInspector] public Vector3 InternalVelocityAdd;

        public static FirstPersonCharacter GetDefault()
        {
            return new FirstPersonCharacter
            {
                WalkSpeed = 6.5f,
                SprintSpeed = 3f,
                CrouchSpeed = 3f,
                AlternativeModeSpeed = 4f,
                RotationSharpness = 10f,
                MovementSharpness = 15f,
                AirAcceleration = 10f,
                AirMaxSpeed = 1f,
                AirDrag = 0.1f,
                JumpSpeed = 10f,
                JumpPreGroundingGraceTime = 0,
                JumpPostGroundingGraceTime = 0,
                MaxFallSpeed = -200f,
                Gravity = Vector3.up * -30f,

                MinVAngle = -89f,
                MaxVAngle = 89f,

                CameraPointHeight = KinematicCharacterUtilities.Constants.MaxCameraHeight,
                GroundingUp = Vector3.up,
            };
        }
    }
}
