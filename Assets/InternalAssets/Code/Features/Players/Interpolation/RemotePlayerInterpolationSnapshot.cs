using System;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Networking.Libs.MirrorInterpolation;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Interpolation
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