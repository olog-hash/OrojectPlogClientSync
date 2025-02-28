using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class CharacterInterpolationProvider : MonoProvider<CharacterInterpolation> 
	{
        private void Awake()
        {
            ref CharacterInterpolation data = ref GetData();
            Transform transform = GetComponent<Transform>();
            data.CurrentTransform.Position = transform.localPosition;
            data.CurrentTransform.Rotation = transform.localRotation;
        }
    }

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct CharacterInterpolation : IComponent 
	{
        public RigidTransform PreviousTransform;
        public RigidTransform CurrentTransform;
        public bool InterpolateTranslation;
        public bool InterpolateRotation;
        public byte InterpolationSkipping;

        public void SkipNextInterpolation()
        {
            InterpolationSkipping |= 1;
            InterpolationSkipping |= 2;
        }

        public void SkipNextTranslationInterpolation()
        {
            InterpolationSkipping |= 1;
        }

        public bool ShouldSkipNextTranslationInterpolation()
        {
            return (InterpolationSkipping & 1) == 1;
        }
    }
}