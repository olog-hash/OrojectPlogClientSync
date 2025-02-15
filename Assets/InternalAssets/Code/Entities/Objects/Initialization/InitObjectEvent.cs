using LiteNetLib.Utils;
using ProjectOlog.Code.Core.Enums;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Initialization
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InitObjectEvent : IComponent
    {
        public int ServerID;
        public ENetworkObjectType ObjectType;

        public Vector3 Position;
        public Quaternion Rotation;

        public NetDataPackage ObjectData;
    }
}