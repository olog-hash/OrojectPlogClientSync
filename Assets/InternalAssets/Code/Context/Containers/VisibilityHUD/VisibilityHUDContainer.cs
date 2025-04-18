using System;
using ProjectOlog.Code.Engine.Cameras.Core;
using ProjectOlog.Code.UI.Core;
using R3;

namespace ProjectOlog.Code.DataStorage.Core.VisibilityHUD
{
    /// <summary>
    /// Контейнер для работы с состояниями видимости камеры предметов (оружейной камеры) и игрового HUD'а.
    /// </summary>
    public sealed class VisibilityHUDContainer : ISceneContainer, IDisposable
    {
        public ReadOnlyReactiveProperty<bool> GlobalHUDVisibility => _globalHUDVisibility.ToReadOnlyReactiveProperty();
        private ReactiveProperty<bool> _globalHUDVisibility = new ReactiveProperty<bool>(true);
        
        public ReadOnlyReactiveProperty<bool> GlobalItemsCameraVisibility => _globalItemsCameraVisibility.ToReadOnlyReactiveProperty();
        private ReactiveProperty<bool> _globalItemsCameraVisibility = new ReactiveProperty<bool>(true);
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        public VisibilityHUDContainer()
        {
            // Подписываемся на изменения и применяем их к соответствующим системам
            _globalHUDVisibility
                .Subscribe(visible => BaseViewModel.SetGlobalVisibility(visible))
                .AddTo(_disposables);
                
            _globalItemsCameraVisibility
                .Subscribe(visible => MainCamera.SetItemsCameraVisibility(visible))
                .AddTo(_disposables);
        }
        
        public void Reset()
        {
            ShowAll();
        }

        // Методы для UI
        public void ShowHUD() => _globalHUDVisibility.Value = true;
        public void HideHUD() => _globalHUDVisibility.Value = false;
        public void SetHUDVisibility(bool visible) => _globalHUDVisibility.Value = visible;
        public void ToggleHUD() => _globalHUDVisibility.Value = !_globalHUDVisibility.Value;
        
        // Методы для камеры предметов
        public void ShowItemsCamera() => _globalItemsCameraVisibility.Value = true;
        public void HideItemsCamera() => _globalItemsCameraVisibility.Value = false;
        public void SetItemsCameraVisibility(bool visible) => _globalItemsCameraVisibility.Value = visible;
        public void ToggleItemsCamera() => _globalItemsCameraVisibility.Value = !_globalItemsCameraVisibility.Value;
        
        // Комбинированные методы
        public void ShowAll()
        {
            ShowHUD();
            ShowItemsCamera();
        }
        
        public void HideAll()
        {
            HideHUD();
            HideItemsCamera();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _globalHUDVisibility.Dispose();
            _globalItemsCameraVisibility.Dispose();
        }
    }
}