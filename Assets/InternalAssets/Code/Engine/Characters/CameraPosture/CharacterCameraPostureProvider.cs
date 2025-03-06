using System.ComponentModel;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using IComponent = Scellecs.Morpeh.IComponent;

namespace ProjectOlog.Code.Engine.Characters.CameraHeightPoint
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [RequireComponent(typeof(TranslationProvider))]
    public sealed class CharacterCameraPostureProvider : MonoProvider<CharacterCameraPostureComponent> 
    {

    }
    
    /// <summary>
    /// Компонент который весит на камере персонажа и регулирует её высоту на основании состояния игрока
    /// (сидит, стоит).
    /// </summary>
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CharacterCameraPostureComponent : IComponent
    {
        public EntityProvider CharacterProvider;
        
        [HideInInspector]
        public float CameraPointHeight;
    }
}