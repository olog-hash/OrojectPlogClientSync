using System;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
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