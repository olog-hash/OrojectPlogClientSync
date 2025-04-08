using System;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.MenuESC.Presenter
{
    public class MenuEscViewModel : BaseViewModel, ILayer
    {
        public const string LAYER_NAME = "MenuEsc";
        
        // Свойство для отслеживания полноэкранного режима
        public ReactiveProperty<bool> IsFullscreen { get; } = new ReactiveProperty<bool>(false);
        
        protected override bool IsCanIgnoreGlobalVisibility => true;
        
        public MenuEscViewModel() : base()
        {
            // Инициализация начального значения полноэкранного режима
            IsFullscreen.Value = Screen.fullScreen;
        }
        
        public void ShowLayer()
        {
            Show();
        }

        public void HideLayer()
        {
            Hide();
        }
        
        // Метод для возврата в игру (закрытие меню)
        public void ReturnToGame()
        {
            CloseLayer();
        }
        
        // Метод для переключения полноэкранного режима
        public void ToggleFullscreen()
        {
            IsFullscreen.Value = !IsFullscreen.Value;
            Screen.fullScreen = IsFullscreen.Value;
            
            CloseLayer();
        }
        
        // Метод для открытия настроек
        public void OpenSettings()
        {
            // Скрываем меню ESC
            CloseLayer();
    
            // Показываем настройки
            if (LayersManager.IsLayerCanBeShown(SettingsViewModel.LAYER_NAME))
            {
                LayersManager.ShowLayer(SettingsViewModel.LAYER_NAME);
            }
        }
        
        // Метод для выхода в главное меню
        public void ExitToMainMenu()
        {
            Debug.Log("Выход в главное меню");
        }

        private void CloseLayer()
        {
            if (LayersManager.IsLayerActive(MenuEscViewModel.LAYER_NAME))
            {
                LayersManager.HideLayer(MenuEscViewModel.LAYER_NAME);
            }
        }
        
        public override void Dispose()
        {
            base.Dispose();
            IsFullscreen.Dispose();
        }
    }
}