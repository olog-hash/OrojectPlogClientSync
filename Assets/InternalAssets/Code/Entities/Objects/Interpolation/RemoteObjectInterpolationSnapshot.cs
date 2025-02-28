using System;
using ProjectOlog.Code._InDevs.Players.RemoteSync;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Snapshot
{
    [Serializable]
    public struct RemoteObjectInterpolationSnapshot : Networking.Libs.MirrorInterpolation.Snapshot
    {
        public double remoteTime { get; set; }
        public double localTime { get; set; }
        
        // Данные игрока
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        
        public RemoteObjectInterpolationSnapshot(double remoteTime, double localTime, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.remoteTime = remoteTime;
            this.localTime = localTime;
            
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public static RemoteObjectInterpolationSnapshot Interpolate(RemoteObjectInterpolationSnapshot from, RemoteObjectInterpolationSnapshot to, double t)
        {
            return new RemoteObjectInterpolationSnapshot(
                0, 0,
                Vector3.Lerp(from.Position, to.Position, (float)t),
                Quaternion.Lerp(from.Rotation, to.Rotation, (float)t),
                Vector3.Lerp(from.Scale, to.Scale, (float)t)
            );
        }
    }
}