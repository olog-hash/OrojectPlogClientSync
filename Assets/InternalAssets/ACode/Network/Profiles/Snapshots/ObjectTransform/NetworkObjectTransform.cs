

using UnityEngine;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots.ObjectTransform
{
    public class NetworkObjectTransform
    {
        public Vector3? Position;
        public Vector3? Rotation;
        public Vector3? Scale;
        
        public bool IsSame(NetworkObjectTransform other)
        {
            bool positionSame = Position.Equals(other.Position);
            bool rotationSame = Rotation.Equals(other.Rotation);
            bool statesSame = Scale.Equals(other.Scale);
            
            return positionSame && rotationSame && statesSame;
        }

        public bool IsSync()
        {
            return Position.HasValue || Rotation.HasValue || Scale.HasValue;
        }
        
        public NetworkObjectTransform Clone()
        {
            return new NetworkObjectTransform
            {
                Position = Position,
                Rotation = Rotation,
                Scale = Scale,
            };
        }
    }
}