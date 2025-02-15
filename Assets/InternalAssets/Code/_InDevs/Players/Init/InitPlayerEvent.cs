using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Init
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InitPlayerEvent : IComponent
    {
        public int ServerID;
        public int UserID;
        public string Username;

        public Vector3 Position;
        public Quaternion Rotation;

        public bool IsDead;
        public bool IsLocalPlayer;
    }
}