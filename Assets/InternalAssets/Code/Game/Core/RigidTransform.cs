using UnityEngine;

namespace ProjectOlog.Code.Game.Core
{
    [System.Serializable]
    public struct RigidTransform
    {
        public Vector3 position;
        public Quaternion rotation;

        public RigidTransform(Quaternion _rotation, Vector3 _position)
        {
            position = _position;
            rotation = _rotation;
        }
    }
}
