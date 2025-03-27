using UnityEngine;

namespace ProjectOlog.Code.Network.Profiles.Snapshots.PlayerTransform
{
    public class NetworkPlayerTransform
    {
        public double RemoteTime;
        
        public Vector3 Position;
        public float YawDegrees;
        public float PitchDegrees;
        
        public bool IsGrounded;
        public byte CharacterBodyState;

        public NetworkPlayerTransform Clone()
        {
            return new NetworkPlayerTransform
            {
                RemoteTime = RemoteTime,

                Position = Position,
                YawDegrees = YawDegrees,
                PitchDegrees = PitchDegrees,

                IsGrounded = IsGrounded,
                CharacterBodyState = CharacterBodyState
            };
        }
    }
}