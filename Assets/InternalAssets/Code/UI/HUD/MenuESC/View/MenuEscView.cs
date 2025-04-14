using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.MenuESC.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.MenuESC.View
{
    public class MenuEscView : UIToolkitScreen<MenuEscViewModel>
    {
        private Button _returnToGameButton;
        private Button _fullscreenButton;
        private Button _settingsButton;
        private Button _exitToMenuButton;

        protected override void SetVisualElements()
        {
            _returnToGameButton = _root.Q<Button>("returnToGame");
            _fullscreenButton = _root.Q<Button>("fullscreen");
            _settingsButton = _root.Q<Button>("settings");
            _exitToMenuButton = _root.Q<Button>("exitToMenu");
        }

        protected override void RegisterButtonCallbacks()
        {
            if (_returnToGameButton != null)
            {
                _returnToGameButton.clicked += OnReturnToGameClicked;
            }

            if (_fullscreenButton != null)
            {
                _fullscreenButton.clicked += OnFullscreenToggleClicked;
            }

            if (_settingsButton != null)
            {
                _settingsButton.clicked += OnSettingsClicked;
            }

            if (_exitToMenuButton != null)
            {
                _exitToMenuButton.clicked += OnExitToMenuClicked;
            }
        }

        private void OnReturnToGameClicked()
        {
            _model?.ReturnToGame();
        }

        private void OnFullscreenToggleClicked()
        {
            _model?.ToggleFullscreen();
        }

        private void OnSettingsClicked()
        {
            _model?.OpenSettings();
        }

        private void OnExitToMenuClicked()
        {
            _model?.ExitToMainMenu();
        }

        protected override void OnBind(MenuEscViewModel model)
        {
            HideOnAwake = true;
            
            // Подписываемся на изменение полноэкранного режима
            model.IsFullscreen
                .Subscribe(isFullscreen =>
                {
                    if (_fullscreenButton != null)
                    {
                        _fullscreenButton.text = isFullscreen ? "ОКОННЫЙ РЕЖИМ" : "НА ВЕСЬ ЭКРАН";
                    }
                })
                .AddTo(_disposables);
        }

        protected override void OnUnbind(MenuEscViewModel model)
        {
            // Важно отписаться от событий при отвязке
            if (_returnToGameButton != null)
                _returnToGameButton.clicked -= OnReturnToGameClicked;

            if (_fullscreenButton != null)
                _fullscreenButton.clicked -= OnFullscreenToggleClicked;

            if (_settingsButton != null)
                _settingsButton.clicked -= OnSettingsClicked;

            if (_exitToMenuButton != null)
                _exitToMenuButton.clicked -= OnExitToMenuClicked;
        }
    }
}