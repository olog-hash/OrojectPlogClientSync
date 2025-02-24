using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Networking.Game.Core
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class NetworkPlayerProvider : MonoProvider<NetworkPlayer> 
	{

	}

	[global::System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct NetworkPlayer : IComponent 
	{
		public int UserID;
		
		/// После респавна - версия должна меняться, чтобы игрок не присылал устаревшие данные своего состояния.
		public ushort LastStateVersion;
    }
}