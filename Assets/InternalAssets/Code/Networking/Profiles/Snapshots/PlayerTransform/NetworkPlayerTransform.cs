using System;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots.NetworkTransformUtilits
{
    public class NetworkPlayerTransform
    {
        public Vector3 Position;
        public float YawDegrees;
        public float PitchDegrees;
        
        public bool IsGrounded;
        public byte CharacterBodyState;

        public NetworkPlayerTransform Clone()
        {
            return new NetworkPlayerTransform
            {
                Position = Position,
                YawDegrees = YawDegrees,
                PitchDegrees = PitchDegrees,
                
                IsGrounded = IsGrounded,
                CharacterBodyState = CharacterBodyState
            };
        }
    }
}