using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Game.Core
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public sealed class TranslationProvider : MonoProvider<Translation> 
	{
        private void Awake()
        {
            ref Translation data = ref GetData();
            data.Transform = GetComponent<Transform>();
        }
    }

	[System.Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public struct Translation : IComponent 
	{
        public Transform Transform;

        #region Lambdas
        public Vector3 position { get => Transform.position; set => Transform.position = value; }
        public Vector3 localPosition { get => Transform.localPosition; set => Transform.localPosition = value; }
        public Quaternion rotation { get => Transform.rotation; set => Transform.rotation = value; }
        public Quaternion localRotation { get => Transform.localRotation; set => Transform.localRotation = value; }
        public Vector3 scale { get => Transform.localScale; set => Transform.localScale = value; }
        #endregion
    }
}