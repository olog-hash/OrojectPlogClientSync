using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Input.PlayerInput.FirstPerson
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class FirstPersonPlayerProvider : MonoProvider<FirstPersonPlayer> 
	{

	}

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct FirstPersonPlayer : IComponent 
	{
        public EntityProvider ControlledCharacter;

        [System.NonSerialized]
        public uint LastInputsProcessingFixedTick;
        [System.NonSerialized]
        public uint LastInputsProcessingTickrateTick;
    }
}