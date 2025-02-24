using System;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Networking.Libs.MirrorInterpolation;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.RemoteSync
{
    [Serializable]
    public struct RemotePlayerInterpolationSnapshot : Snapshot
    {
        public double remoteTime { get; set; }
        public double localTime { get; set; }
        
        // Данные игрока
        public Vector3 Position;
        public Quaternion Rotation;

        // Остальное
        public float ViewPitchDegrees;
        public bool IsGrounded;
        public ECharacterBodyState CharacterBodyState;

        public RemotePlayerInterpolationSnapshot(double remoteTime, double localTime, Vector3 position, Quaternion rotation, float viewPitchDegrees, bool isGrounded, ECharacterBodyState characterBodyState)
        {
            this.remoteTime = remoteTime;
            this.localTime = localTime;
            Position = position;
            Rotation = rotation;
            ViewPitchDegrees = viewPitchDegrees;
            IsGrounded = isGrounded;
            CharacterBodyState = characterBodyState;
        }

        public static RemotePlayerInterpolationSnapshot Interpolate(RemotePlayerInterpolationSnapshot from, RemotePlayerInterpolationSnapshot to, double t)
        {
            // Определяем, сколько кадров между снапшотами для более плавной интерполяции состояний
            bool shouldInterpolateState = Mathf.Abs((float)(to.remoteTime - from.remoteTime)) < 0.1f; // 100ms threshold

            return new RemotePlayerInterpolationSnapshot(
                0, 0,
                Vector3.Lerp(from.Position, to.Position, (float)t),
                Quaternion.Lerp(from.Rotation, to.Rotation, (float)t),
                Mathf.Lerp(from.ViewPitchDegrees, to.ViewPitchDegrees, (float)t),
                // Для IsGrounded используем состояние, которое ближе к текущему времени интерполяции
                t > 0.5f ? to.IsGrounded : from.IsGrounded,
                // Для CharacterBodyState используем более умную логику
                shouldInterpolateState ? to.CharacterBodyState : 
                t > 0.5f ? to.CharacterBodyState : from.CharacterBodyState
            );
        }
    }
}

/*
private Vector2 CalculateMoveVector(Vector3 previousPosition, Vector3 currentPosition,
            Quaternion currentRotation)
        {
            float deltaX = currentPosition.x - previousPosition.x;
            float deltaZ = currentPosition.z - previousPosition.z;

            Vector2 inputMoveDirection = new Vector2(deltaX, deltaZ);

            float rotationAngle = currentRotation.eulerAngles.y * Mathf.Deg2Rad;

            float sin = Mathf.Sin(rotationAngle);
            float cos = Mathf.Cos(rotationAngle);

            Vector2 rotatedMoveDirection = new Vector2(
                inputMoveDirection.x * cos - inputMoveDirection.y * sin,
                inputMoveDirection.x * sin + inputMoveDirection.y * cos
            );

            rotatedMoveDirection.x = -rotatedMoveDirection.x;

            return rotatedMoveDirection;
        }
 */