using System;
using ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Entities.Objects.Instantiate
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InstantiateObjectEvent : IComponent
    {
        public InstantiateObjectPacket InstantiateObjectPacket;
        public EntityProviderMappingPool EntityProviderMappingPool;
    }
}