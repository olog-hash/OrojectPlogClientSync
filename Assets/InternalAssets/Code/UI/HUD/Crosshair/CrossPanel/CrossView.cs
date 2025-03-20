using ProjectOlog.Code.UI.Core;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel
{
    public class CrossView: AbstractScreen<CrossViewModel>
    {
        [SerializeField] private Image _crossImage;

        private Color _enemyTargetColor = new Color(1f, 0.44f, 0.44f);
        private Color _defaultTargetColor = Color.white;
        
        protected override void OnBind(CrossViewModel model)
        {
            // Подписываемся на изменение состояния прицела
            model.IsTargetOnAim
                .Subscribe(isTargetOnAim =>
                {
                    _crossImage.color = isTargetOnAim ? _enemyTargetColor : _defaultTargetColor;
                })
                .AddTo(_disposables);
        }

        protected override void OnUnbind(CrossViewModel model)
        {
            // Отписки автоматически обрабатываются через _disposables.Clear() в базовом классе
        }
    }
}