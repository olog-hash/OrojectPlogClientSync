using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.View
{
    public class DamageScreenView : AbstractScreen<DamageScreenViewModel>
    {
        [SerializeField] private Image _damageOverlayImage;
        
        protected override void OnBind(DamageScreenViewModel model)
        {
            // Подписываемся на изменения альфы
            model.DamageEffectAlpha
                .Subscribe(UpdateDamageVisual)
                .AddTo(_disposables);
                
            // Инициализируем начальную альфу
            if (_damageOverlayImage != null)
            {
                Color color = _damageOverlayImage.color;
                color.a = 0f;
                _damageOverlayImage.color = color;
            }
        }
        
        protected override void OnUnbind(DamageScreenViewModel model)
        {
            // Сбрасываем альфу при отвязке
            if (_damageOverlayImage != null)
            {
                Color color = _damageOverlayImage.color;
                color.a = 0f;
                _damageOverlayImage.color = color;
            }
        }
        
        // Обновление прозрачности изображения
        private void UpdateDamageVisual(float alpha)
        {
            if (_damageOverlayImage != null)
            {
                Color color = _damageOverlayImage.color;
                color.a = alpha;
                _damageOverlayImage.color = color;
            }
        }
        
        protected override void ApplyVisibility(bool isVisible)
        {
            // Не меняем активность объекта, только альфу
            if (_damageOverlayImage != null && !isVisible)
            {
                Color color = _damageOverlayImage.color;
                color.a = 0f;
                _damageOverlayImage.color = color;
            }
        }
        
        // Вызываем Update у ViewModel
        public override void OnUpdate(float deltaTime)
        {
            if (_model != null)
            {
                _model.Update(deltaTime);
            }
        }
    }
}