using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Engine.Characters.PlayerInput.FirstPerson
{
	/// <summary>
	/// Провайдер для компонента игрока от первого лица.
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class FirstPersonPlayerProvider : MonoProvider<FirstPersonPlayer> 
	{

	}

	/// <summary>
	/// Компонент, представляющий игрока с управлением от первого лица.
	/// Содержит ссылку на управляемый персонаж и данные для обработки ввода.
	/// </summary>
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