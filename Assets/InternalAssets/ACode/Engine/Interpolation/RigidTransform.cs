using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectOlog.Code.Game.Core
{
    /// <summary>
    /// Структура, объединяющая позицию и поворот для слота данных в интерполяции
    /// </summary>
    [System.Serializable]
    public struct RigidTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public RigidTransform(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
