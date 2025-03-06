using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Utilits;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Characters.CameraHeightPoint
{
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CharacterCameraPostureSystem : UpdateSystem
    {
        private Filter _cameraPostureFilter;

        public override void OnAwake()
        {
            _cameraPostureFilter = World.Filter.With<CharacterCameraPostureComponent>().With<Translation>().Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _cameraPostureFilter)
            {
                ref var translation = ref entity.GetComponent<Translation>();
                ref var cameraPostureComponent = ref entity.GetComponent<CharacterCameraPostureComponent>();

                if (TryGetCharacterBodyState(cameraPostureComponent.CharacterProvider, out var characterBodyState))
                {
                    bool isCrouch = characterBodyState == ECharacterBodyState.Crouch;
                    
                    CharacterControlUtilities.CameraPositionUpdate(ref cameraPostureComponent.CameraPointHeight, isCrouch, deltaTime);
                }
                else
                {
                    cameraPostureComponent.CameraPointHeight = KinematicCharacterUtilities.Constants.MaxCameraHeight;
                }
                
                translation.localPosition = cameraPostureComponent.CameraPointHeight * Vector3.up;
            }
        }

        private bool TryGetCharacterBodyState(EntityProvider entityProvider, out ECharacterBodyState characterBodyState)
        {
            characterBodyState = default;
            
            if (entityProvider == null || entityProvider.Entity == null ||
                entityProvider.Entity.IsNullOrDisposed()) return false;

            if (entityProvider.Entity.TryGetComponent(out CharacterBodyLogger characterBodyLogger))
            {
                characterBodyState = characterBodyLogger.CharacterBodyState;
                return true;
            }

            return false;
        }
    }
}