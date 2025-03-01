using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Network.Gameplay.Core.Components
{
    /// <summary>
    /// Провайдер компонента сетевой идентификации объекта.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NetworkIdentityProvider : MonoProvider<NetworkIdentity> 
    {
    }

    /// <summary>
    /// Компонент, хранящий уникальный идентификатор сетевого объекта, назначенный сервером.
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NetworkIdentity : IComponent 
    {
        /// <summary>
        /// Уникальный серверный идентификатор объекта.
        /// </summary>
        public ushort ServerID;
    }
}