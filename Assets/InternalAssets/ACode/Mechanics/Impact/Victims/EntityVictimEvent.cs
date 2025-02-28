using System;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Mechanics.Impact.Victims
{
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EntityVictimEvent : IComponent
    {
        public Entity VictimEntity;

        public bool IsNetworkPlayer()
        {
            return VictimEntity.Has<NetworkPlayer>();
        }

        public bool IsLocalPlayer()
        {
            return VictimEntity.Has<LocalPlayerMarker>();
        }
    }
}