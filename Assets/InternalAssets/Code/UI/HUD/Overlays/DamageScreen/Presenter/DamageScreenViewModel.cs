using ProjectOlog.Code.UI.Core;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.Presenter
{
    public class DamageScreenViewModel : BaseViewModel
    {
        // Значение прозрачности эффекта
        private ReactiveProperty<float> _damageEffectAlpha = new ReactiveProperty<float>(0f);
        public ReadOnlyReactiveProperty<float> DamageEffectAlpha => _damageEffectAlpha;

        // Базовые настройки анимации
        public float FadeInDuration { get; set; } = 0.1f;      // Время появления эффекта
        
        // Настройки для масштабирования эффекта
        public float MinDamage { get; set; } = 1f;            // Минимальный урон для заметного эффекта
        public float MaxAccumulatedDamage { get; set; } = 15f; // Максимальный накопленный урон 
        public float MinAlpha { get; set; } = 0.2f;           // Минимальная альфа при слабом уроне
        public float MaxAlpha { get; set; } = 0.9f;           // Максимальная альфа при сильном уроне
        
        // Настройки затухания накопительного эффекта
        public float DamageDecayRate { get; set; } = 20f;      // Скорость затухания урона (HP в секунду)
        public float AlphaDecayRate { get; set; } = 0.3f;      // Базовая скорость затухания альфы
        
        // Настройки для плавного затухания
        public float MinimumFadeOutDuration { get; set; } = 0.8f; // Минимальное время полного затухания
        public float FadeOutThreshold { get; set; } = 0.2f;       // Порог для более плавного затухания
        
        private float _accumulatedDamage = 0f;     // Накопленный урон
        private bool _isEffectActive = false;      // Активен ли эффект
        private float _currentAlpha = 0f;          // Текущее значение альфы
        private float _targetAlpha = 0f;           // Целевое значение альфы
        private bool _isFadingOut = false;         // Находимся ли мы в режиме окончательного затухания
        private float _fadeOutStartTime = 0f;      // Время начала финального затухания
        private float _fadeOutStartAlpha = 0f;     // Начальная альфа для финального затухания
        
        // Игнорируем глобальную видимость UI для эффекта урона
        protected override bool IsCanIgnoreGlobalVisibility => true;

        // Активация эффекта при получении урона
        public void ApplyDamage(float damageAmount)
        {
            // Игнорируем слишком маленький урон
            if (damageAmount < MinDamage * 0.5f)
                return;
                
            // Добавляем урон к накопленному значению с ограничением максимума
            _accumulatedDamage += damageAmount;
            _accumulatedDamage = Mathf.Min(_accumulatedDamage, MaxAccumulatedDamage);
            
            // Активируем эффект, если он ещё не активен
            if (!_isEffectActive)
            {
                _isEffectActive = true;
                _isFadingOut = false; // Отменяем режим финального затухания
                Show();
            }
            
            // Если были в режиме финального затухания, выходим из него
            if (_isFadingOut)
            {
                _isFadingOut = false;
            }
            
            // Рассчитываем целевую альфу на основе накопленного урона
            UpdateTargetAlpha();
        }
        
        // Обновление целевой альфы на основе накопленного урона
        private void UpdateTargetAlpha()
        {
            // Если мы в режиме финального затухания, не обновляем целевую альфу
            if (_isFadingOut)
                return;
                
            // Нормализуем урон в диапазон [0, 1]
            float normalizedDamage = Mathf.Clamp01((_accumulatedDamage - MinDamage) / (MaxAccumulatedDamage - MinDamage));
            
            // Используем кривую мощности для более выразительного эффекта
            normalizedDamage = Mathf.Pow(normalizedDamage, 0.8f); // Немного сглаживаем кривую
            
            // Рассчитываем целевую альфу
            _targetAlpha = Mathf.Lerp(MinAlpha, MaxAlpha, normalizedDamage);
        }
        
        // Логика анимации и обновления состояния
        public void Update(float deltaTime)
        {
            // Если эффект неактивен, выходим
            if (!_isEffectActive)
                return;
                
            // Обрабатываем финальное затухание
            if (_isFadingOut)
            {
                float elapsedTime = Time.time - _fadeOutStartTime;
                float progress = elapsedTime / MinimumFadeOutDuration;
                
                // Используем плавную кривую затухания
                float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
                
                // Затухание от начальной альфы до нуля
                _currentAlpha = Mathf.Lerp(_fadeOutStartAlpha, 0f, smoothProgress);
                
                // Применяем рассчитанную альфу
                _damageEffectAlpha.Value = _currentAlpha;
                
                // Полностью завершаем эффект, когда затухание закончено
                if (progress >= 1f)
                {
                    _isEffectActive = false;
                    _isFadingOut = false;
                    _currentAlpha = 0f;
                    _targetAlpha = 0f;
                    _damageEffectAlpha.Value = 0f;
                }
                
                return;
            }
            
            // Уменьшаем накопленный урон со временем
            _accumulatedDamage -= DamageDecayRate * deltaTime;
            
            // Если урон полностью затух
            if (_accumulatedDamage <= 0f)
            {
                // Если не в режиме финального затухания, запускаем его
                if (!_isFadingOut && _currentAlpha > 0f)
                {
                    _isFadingOut = true;
                    _fadeOutStartTime = Time.time;
                    _fadeOutStartAlpha = _currentAlpha;
                }
                
                _accumulatedDamage = 0f;
            }
            else
            {
                // Обновляем целевую альфу в соответствии с изменением урона
                UpdateTargetAlpha();
                
                // Плавное изменение текущей альфы к целевой
                if (_currentAlpha < _targetAlpha)
                {
                    // Быстрое нарастание (зависит от разницы)
                    float riseSpeed = 1f / FadeInDuration;
                    _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha, riseSpeed * deltaTime);
                }
                else if (_currentAlpha > _targetAlpha)
                {
                    // Основное затухание
                    float decayMultiplier = 1f;
                    
                    // Если альфа ниже порога, затухаем медленнее
                    if (_currentAlpha < FadeOutThreshold)
                    {
                        decayMultiplier = _currentAlpha / FadeOutThreshold; // Замедляем ближе к нулю
                        decayMultiplier = Mathf.Max(0.1f, decayMultiplier); // Но не медленнее определенного значения
                    }
                    
                    // Применяем затухание с учетом модификатора
                    float effectiveDecayRate = AlphaDecayRate * decayMultiplier;
                    _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, effectiveDecayRate * deltaTime);
                }
                
                // Если альфа становится очень низкой и урон малый, запускаем финальное затухание
                if (_currentAlpha < 0.05f && _accumulatedDamage < MinDamage)
                {
                    _isFadingOut = true;
                    _fadeOutStartTime = Time.time;
                    _fadeOutStartAlpha = _currentAlpha;
                }
                
                // Применяем рассчитанную альфу
                _damageEffectAlpha.Value = _currentAlpha;
            }
        }
        
        public override void Dispose()
        {
            _damageEffectAlpha.Dispose();
            base.Dispose();
        }
    }
}
