using System;
using System.Collections.Generic;
using ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.Instantiate
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InstantiatePlayerEvent : IComponent
    {
        public InstantiatePlayerPacket InstantiatePlayerPacket;
        public EntityProviderMappingPool EntityProviderMappingPool;
    }
}