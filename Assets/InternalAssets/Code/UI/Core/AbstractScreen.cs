using System;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Core
{
    /// <summary>
    /// Базовый экран, связывающий View с ViewModel определённого типа.
    /// </summary>
    public abstract class AbstractScreen<TModel> : BaseScreen where TModel:IViewModel
    {
        /// <summary>
        /// Скрывает интерфейс сразу после подключения.
        /// </summary>
        public bool HideOnAwake { get; protected set; }

        public override Type ModelType => typeof(TModel);
    
        protected TModel _model { get; private set; }

        /// <summary>
        /// Вызывается вручную каждый раз в методе Bind после основной инициализации
        /// </summary>
        public virtual void Initialize() { }

        public override void Show() => ApplyVisibility(true);
        public override void Hide() => ApplyVisibility(false);

        protected virtual void ApplyVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        // Производим обновление если View часть активна
        private void Update()
        {
            if (_model != null && _model.IsVisible.CurrentValue)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {
            
        }

        public override void Bind(object model)
        {
            if (model is TModel typedModel)
            {
                Bind(typedModel);
            }
        }

        public override void Unbind()
        {
            if (_model != null)
            {
                OnUnbind(_model);
            }
            _disposables.Clear();
        }
        
        public void Bind(TModel model)
        {
            _model = model;
            
            // Подписываемся на изменения видимости модели
            model.IsVisible
                .Subscribe(ApplyVisibility)
                .AddTo(_disposables);
            
            // Выполняем инициализацию
            Initialize();
            OnBind(model);
            
            if (HideOnAwake)
            {
                model.Hide();
            }
            
            // Устанавливаем начальное состояние видимости
            ApplyVisibility(model.IsVisible.CurrentValue);
        }

        public void Unbind(TModel model)
        {
            _model = default(TModel);
            OnUnbind(model);
        }

        // Методы которые реализуются в наследниках при привязке/отвязке.
        // Они вызываются уже после основной логики отвязки.
        protected abstract void OnBind(TModel model);
        protected abstract void OnUnbind(TModel model);
    }
}