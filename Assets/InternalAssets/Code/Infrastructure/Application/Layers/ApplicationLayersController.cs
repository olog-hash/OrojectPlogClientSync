using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    public class ApplicationLayersController : IUpdate
    {
        private static LayerInfo CurrentLayerInfo;
        
        public ApplicationLayersController(RuntimeHelper runtimeHelper)
        {
            Reset();
            
            runtimeHelper.RegisterUpdate(this);
        }

        public static void Reset()
        {
            CurrentLayerInfo = LayerInfo.None;
        }

        public static void ChangeState(LayerInfo newLayer)
        {
            CurrentLayerInfo = newLayer;
            EnterState();
        }
        
        public static void EnterState()
        {
            InputControls.IsMouseControlEnabled = CurrentLayerInfo.IsMouseControlEnabled;
            InputControls.IsKeyControlEnabled = CurrentLayerInfo.IsKeyControlEnabled;
            
            Cursor.lockState = CurrentLayerInfo.IsCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        public static void UpdateCurrentState()
        {
            if (CurrentLayerInfo.IsCursorLocked)
            {
                if (InputControls.GetMouseButtonDown(0) || InputControls.GetMouseButtonDown(1))
                {
                   // Cursor.lockState = CursorLockMode.Locked;
                }
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                  //  Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            UpdateCurrentState();
        }
    }
}