using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    public class ControlsTabController : UIToolkitElementView
    {
        private ControlsSettingsModel _model;
    
        public ControlsTabController(VisualElement root) : base(root) { }
    
        public void Bind(ControlsSettingsModel model)
        {
            _model = model;
            // Реализация для вкладки управления
        }
    }
}