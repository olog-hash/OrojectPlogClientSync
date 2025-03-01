using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Engine.Interpolation
{
    /// <summary>
    /// Провайдер для компонента интерполяции, инициализирующий его состояние
    /// </summary>
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class InterpolationProvider : MonoProvider<Interpolation> 
	{
        private void Awake()
        {
            ref Interpolation data = ref GetData();
            UnityEngine.Transform transform = GetComponent<UnityEngine.Transform>();
            data.CurrentTransform.Position = transform.localPosition;
            data.CurrentTransform.Rotation = transform.localRotation;
        }
    }

    /// <summary>
    /// Компонент для плавной интерполяции движения объектов между кадрами
    /// </summary>
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
        
        /// <summary>
        /// Битовая маска для пропуска интерполяции:
        /// Бит 0 (1) - пропуск интерполяции позиции
        /// Бит 1 (2) - пропуск интерполяции вращения
        /// </summary>
        public byte InterpolationSkipping;
        
        public void SkipNextInterpolation()
        {
            InterpolationSkipping |= 1; // Устанавливаем бит для позиции
            InterpolationSkipping |= 2; // Устанавливаем бит для вращения
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