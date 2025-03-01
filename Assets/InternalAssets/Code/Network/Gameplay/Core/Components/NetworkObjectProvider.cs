using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Network.Gameplay.Core.Components
{
	/// <summary>
	/// Провайдер компонента для определения типа сетевого объекта.
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class NetworkObjectProvider : MonoProvider<NetworkObject> 
	{
	}

	/// <summary>
	/// Компонент, указывающий тип сетевого объекта.
	/// </summary>
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct NetworkObject : IComponent 
	{
		/// <summary>
		/// Тип сетевого объекта, определяющий его.
		/// </summary>
		[Header("Type of network object (if exists)")]
		public ENetworkObjectType ObjectType;
	}
}