using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Input.PlayerInput.FirstPerson
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class FirstPersonInputsProvider : MonoProvider<FirstPersonInputs> 
	{

	}

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct FirstPersonInputs : IComponent 
	{
        public Vector3 MoveVector;
        public Vector2 LookYawPitchDegrees;

		public bool IsFirePressing;
        public bool IsFireLocked;

        public bool IsAltFirePressing;
        public bool IsAltFireLocked;

        public bool IsMiddlePressing;
        public bool IsMiddleLocked;

		public bool IsUseLocked;

        public bool SprintRequested;
        public bool CrouchRequested;
        public bool JumpRequested;
        public bool NoClipRequested;
    }
}