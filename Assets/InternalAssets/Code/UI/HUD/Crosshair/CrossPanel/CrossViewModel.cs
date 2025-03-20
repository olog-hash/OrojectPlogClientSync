using System;
using ProjectOlog.Code.UI.Core;
using R3;

namespace ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel
{
    public class CrossViewModel : BaseViewModel
    {
        private ReactiveProperty<bool> _isTargetOnAim = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveProperty<bool> IsTargetOnAim => _isTargetOnAim.ToReadOnlyReactiveProperty();
        
        public CrossViewModel()
        {
            // Конструктор уже вызывает базовый BaseViewModel(), где инициализируется видимость
        }
        
        public void SetTargetOnAim(bool value)
        {
            _isTargetOnAim.Value = value;
        }
    }
}