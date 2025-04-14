using System;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.Application.StateMachine;
using ProjectOlog.Code.Infrastructure.Application.StateMachine.States;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.MenuESC.Presenter
{
    public class MenuEscViewModel : LayerViewModel
    {
        protected override bool IsCanIgnoreGlobalVisibility => true;
        
        // Свойство для отслеживания полноэкранного режима
        public ReactiveProperty<bool> IsFullscreen { get; } = new ReactiveProperty<bool>(Screen.fullScreen);

        private ApplicationStateMachine _applicationStateMachine;
        
        public MenuEscViewModel(ApplicationStateMachine applicationStateMachine) : base()
        {
            _applicationStateMachine = applicationStateMachine;
            
            IsFullscreen.Value = Screen.fullScreen;
        }
        
        public void ReturnToGame()
        {
            HideLayerItself();
        }
        
        public void ToggleFullscreen()
        {
            IsFullscreen.Value = !IsFullscreen.Value;
            Screen.fullScreen = IsFullscreen.Value;
            
            HideLayerItself();
        }
        
        public void OpenSettings()
        {
            // Скрываем меню ESC
            HideLayerItself();
    
            // Показываем настройки
            if (_layersManager.IsLayerCanBeShown(nameof(SettingsViewModel)))
            {
                _layersManager.ShowLayer(nameof(SettingsViewModel));
            }
        }
        
        public void ExitToMainMenu()
        {
            _applicationStateMachine.Enter<LeaveBattleLevelState>();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            IsFullscreen.Dispose();
        }
    }
}