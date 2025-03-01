using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Respawn
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct RespawnPlayerEvent : IComponent
    {
        public EntityProvider PlayerProvider;
        public ushort LastStateVersion;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}