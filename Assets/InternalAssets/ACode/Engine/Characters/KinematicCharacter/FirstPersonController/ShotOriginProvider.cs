using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class ShotOriginProvider : MonoProvider<ShotOrigin> 
	{

	}

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct ShotOrigin : IComponent 
	{
		public Translation ShotOriginTranslation;
	}
}