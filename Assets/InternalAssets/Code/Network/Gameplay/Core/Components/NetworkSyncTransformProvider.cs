using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Network.Gameplay.Core.Components
{
    /// <summary>
    /// Провайдер компонента для синхронизации трансформации объекта в сетевом окружении.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NetworkSyncTransformProvider : MonoProvider<NetworkSyncTransform>
    {
    }

    /// <summary>
    /// Компонент, определяющий, какие свойства трансформации объекта должны синхронизироваться по сети.
    /// </summary>
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NetworkSyncTransform : IComponent
    {
        /// <summary>
        /// Трансформ объекта для синхронизации. Если не указан, используется трансформ самой сущности.
        /// </summary>
        [Header("Target")]
        public EntityProvider Target;
        
        [Header("Selective Sync")]
        public bool SyncPosition;
        public bool SyncRotation;
        public bool SyncScale;
    }
}