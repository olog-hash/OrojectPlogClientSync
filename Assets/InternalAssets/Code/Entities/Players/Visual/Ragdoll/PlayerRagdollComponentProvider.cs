using ProjectOlog.Code.Game.Ragdoll;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Visual.Ragdoll
{

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerRagdollComponentProvider : MonoProvider<PlayerRagdollComponent>
    {

    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PlayerRagdollComponent : IComponent
    {
        public Animator Animator;
        public RagdollOperations RagdollOperations;
    }
}