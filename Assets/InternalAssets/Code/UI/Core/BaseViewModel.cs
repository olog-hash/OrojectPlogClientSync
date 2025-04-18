﻿using System;
using R3;

namespace ProjectOlog.Code.UI.Core
{
    public interface IViewModel : IDisposable
    {
        public ReadOnlyReactiveProperty<bool> IsVisible { get; }

        public ReadOnlyReactiveProperty<bool> IsLocalVisible { get; }
        public void Show();
        public void Hide();
    }

    /// <summary>
    /// Базовый класс для всех ViewModel с упрощённой логикой видимости.
    /// Логика показа/скрытия вынесена в комбинированный стрим, который автоматически вызывает OnShow/OnHide.
    /// </summary>
    public abstract class BaseViewModel : IViewModel
    {
        // Комбинированная видимость с учётом глобального состояния
        public ReadOnlyReactiveProperty<bool> IsVisible => _combinedVisibility;
        private ReactiveProperty<bool> _combinedVisibility = new ReactiveProperty<bool>(true);
        
        // Статичная глобальная видимость
        public static ReadOnlyReactiveProperty<bool> GlobalVisibility => _globalVisibility;
        private static ReactiveProperty<bool> _globalVisibility = new ReactiveProperty<bool>(true);
        
        // Локальная видимость (управляется непосредственно моделью)
        public ReadOnlyReactiveProperty<bool> IsLocalVisible => _localVisibility;
        private ReactiveProperty<bool> _localVisibility = new ReactiveProperty<bool>(true);
        
        // Подписки для этого абстрактного класса и наследников
        private CompositeDisposable _personalDisposables = new CompositeDisposable();
        protected CompositeDisposable _disposables = new CompositeDisposable();

        /// <summary>
        /// Позволяет игнорировать глобальное состояние видимости.
        /// </summary>
        protected virtual bool IsCanIgnoreGlobalVisibility => false;
        
        public BaseViewModel()
        {
            _localVisibility
                .Subscribe(_ => UpdateVisibility())
                .AddTo(_personalDisposables);
            
            _globalVisibility
                .Subscribe(_ => UpdateVisibility())
                .AddTo(_personalDisposables);

            // При изменении итоговой видимости вызываем соответствующие методы
            _combinedVisibility
                .DistinctUntilChanged()
                .Subscribe(visible =>
                {
                    if (visible)
                        OnShow();
                    else
                        OnHide();
                })
                .AddTo(_personalDisposables);
            
            // Инициализируем начальное значение
            UpdateVisibility();
        }
        
        // Статичные методы для управления глобальной видимостью
        public static void ResetGlobalVisibility() => _globalVisibility.Value = true;
        public static void ToggleGlobalVisibility() => _globalVisibility.Value = !_globalVisibility.Value;
        public static void SetGlobalVisibility(bool isVisible) => _globalVisibility.Value = isVisible;
        
        // Метод для обновления итоговой видимости
        private void UpdateVisibility()
        {
            _combinedVisibility.Value = _localVisibility.Value && (_globalVisibility.Value || IsCanIgnoreGlobalVisibility);
        }
        
        // Методы Show/Hide теперь просто обновляют локальную видимость
        public void Show() => _localVisibility.Value = true;
        public void Hide() => _localVisibility.Value = false;
        
        public void SetVisible(bool flag)
        {
            if (flag) Show(); 
            else Hide();
        }

        // Методы для наследования при изменении видимости
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        
        public virtual void Dispose()
        {
            IsVisible.Dispose();
            IsLocalVisible.Dispose();
            
            _localVisibility.Dispose();
            _combinedVisibility.Dispose();
            _personalDisposables.Dispose();
            _disposables.Dispose();
        }
    }
}