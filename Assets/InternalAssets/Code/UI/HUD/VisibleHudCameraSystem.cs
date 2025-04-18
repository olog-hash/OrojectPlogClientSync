using ProjectOlog.Code.DataStorage.Core.VisibilityHUD;
using ProjectOlog.Code.Engine.Cameras.Core;
using ProjectOlog.Code.UI.Core;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class VisibleHudCameraSystem : UpdateSystem
    {
        private VisibilityHUDContainer _visibilityHUDContainer;
        
        private bool _isActive = true;

        public VisibleHudCameraSystem(VisibilityHUDContainer visibilityHUDContainer)
        {
            _visibilityHUDContainer = visibilityHUDContainer;
        }

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                if (_isActive)
                {
                    _isActive = !_isActive;
                    
                    if (UnityEngine.Input.GetKey(KeyCode.LeftControl))
                    {
                        _visibilityHUDContainer.SetItemsCameraVisibility(_isActive);
                    }
                    
                    _visibilityHUDContainer.SetHUDVisibility(_isActive);
                }
                else
                {
                    _isActive = !_isActive;
                    
                    _visibilityHUDContainer.SetHUDVisibility(_isActive);
                    _visibilityHUDContainer.SetItemsCameraVisibility(_isActive);
                }
            }
        }
    }
}