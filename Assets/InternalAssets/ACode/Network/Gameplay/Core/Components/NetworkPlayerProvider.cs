using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Networking.Game.Core
{
	/// <summary>
	/// Провайдер компонента NetworkPlayer для сетевого представления игрока в системе ECS.
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class NetworkPlayerProvider : MonoProvider<NetworkPlayer> 
	{
	}

	/// <summary>
	/// Компонент, представляющий сетевого игрока и отслеживающий версию его состояния.
	/// </summary>
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct NetworkPlayer : IComponent 
	{
		/// <summary>
		/// Уникальный идентификатор игрока в сети.
		/// </summary>
		public byte UserID;

		/// <summary>
		/// Версия текущего состояния игрока.
		/// После респавна версия должна увеличиваться, чтобы предотвратить применение устаревших данных.
		/// </summary>
		public ushort LastStateVersion;
	}
}