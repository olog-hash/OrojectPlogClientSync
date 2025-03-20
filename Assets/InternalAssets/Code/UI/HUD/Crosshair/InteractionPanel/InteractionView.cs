using ProjectOlog.Code.UI.Core;
using R3;
using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel
{
    public class InteractionView: AbstractScreen<InteractionViewModel>
    {
        [SerializeField] private TextMeshProUGUI ObjectName;
        [SerializeField] private TextMeshProUGUI ObjectAction;
        [SerializeField] private TextMeshProUGUI ObjectDescription;
        
        protected override void OnBind(InteractionViewModel model)
        {
            HideOnAwake = true;
            
            // Подписываемся на изменения в модели представления
            model.InteractionObjectName
                .Subscribe(name => ObjectName.text = name)
                .AddTo(_disposables);
                
            model.InteractionActionName
                .Subscribe(action => ObjectAction.text = action)
                .AddTo(_disposables);
                
            model.InteractionObjectDescription
                .Subscribe(description => ObjectDescription.text = description)
                .AddTo(_disposables);
        }

        protected override void OnUnbind(InteractionViewModel model)
        {
            // Отписки автоматически обрабатываются через _disposables.Clear() в базовом классе
        }
    }
}