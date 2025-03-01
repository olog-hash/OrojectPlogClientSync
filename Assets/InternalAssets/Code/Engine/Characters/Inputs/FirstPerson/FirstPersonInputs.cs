using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.PlayerInput.FirstPerson
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class FirstPersonInputsProvider : MonoProvider<FirstPersonInputs> 
	{

	}

	/// <summary>
	/// Компонент, хранящий информацию о пользовательском вводе для персонажа от первого лица.
	/// </summary>
	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct FirstPersonInputs : IComponent 
	{
		// Данные перемещения и обзора
		public Vector3 MoveVector;
		public Vector2 LookYawPitchDegrees;

		// Состояние основного действия (левая кнопка мыши)
		public bool IsFirePressing;
		public bool IsFireLocked;

		// Состояние альтернативного действия (правая кнопка мыши)
		public bool IsAltFirePressing;
		public bool IsAltFireLocked;

		// Состояние среднего действия (средняя кнопка мыши)
		public bool IsMiddlePressing;
		public bool IsMiddleLocked;

		// Состояние кнопки использования
		public bool IsUseLocked;

		// Запросы на действия персонажа
		public bool SprintRequested;
		public bool CrouchRequested;
		public bool JumpRequested;
		public bool NoClipRequested;
    }
}