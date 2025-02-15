using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.Core;
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
        private bool _isActive = true;
        
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
                        MainCamera.Instance.ShowItemsCamera(_isActive);
                    }
                    
                    MainCamera.Instance.ShowHudCamera(_isActive);
                }
                else
                {
                    _isActive = !_isActive;
                    
                    MainCamera.Instance.ShowItemsCamera(_isActive);
                    MainCamera.Instance.ShowHudCamera(_isActive);
                }
            }
        }
    }
}