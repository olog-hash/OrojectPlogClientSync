using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Контроллер отвечающий за исполнения логики текущего (доминантного слоя).
    /// Инкапсулирует логику применения настроек влияющие на управления в игре исходя из информации текущего слоя.
    /// </summary>
    public class ApplicationLayersController : IUpdate
    {
        private LayerInputMode _currentLayerInputMode;
        
        public ApplicationLayersController(RuntimeHelper runtimeHelper)
        {
            runtimeHelper.RegisterUpdate(this);
        }

        public void Reset()
        {
            _currentLayerInputMode = LayerInputMode.None;
        }

        public void ChangeState(LayerInputMode newLayer)
        {
            _currentLayerInputMode = newLayer;
            EnterState();
        }
        
        public void EnterState()
        {
            InputControls.IsMouseControlEnabled = _currentLayerInputMode.IsMouseControlEnabled;
            InputControls.IsKeyControlEnabled = _currentLayerInputMode.IsKeyControlEnabled;
            
            Cursor.lockState = _currentLayerInputMode.IsCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
        
        public void UpdateCurrentState()
        {
            if (_currentLayerInputMode.IsCursorLocked)
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