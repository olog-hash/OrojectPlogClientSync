using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Core
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class InterpolationProvider : MonoProvider<Interpolation> 
	{
        private void Awake()
        {
            ref Interpolation data = ref GetData();
            Transform transform = GetComponent<Transform>();
            data.CurrentTransform.position = transform.localPosition;
            data.CurrentTransform.rotation = transform.localRotation;
        }
    }

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct Interpolation : IComponent 
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

        public void SkipNextRotationInterpolation()
        {
            InterpolationSkipping |= 2;
        }

        public bool ShouldSkipNextTranslationInterpolation()
        {
            return (InterpolationSkipping & 1) == 1;
        }

        public bool ShouldSkipNextRotationInterpolation()
        {
            return (InterpolationSkipping & 2) == 2;
        }
    }
}