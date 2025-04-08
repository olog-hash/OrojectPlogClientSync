using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    public class AudioTabController : UIToolkitElementView
    {
        private AudioSettingsModel _model;
    
        public AudioTabController(VisualElement root) : base(root) { }
    
        public void Bind(AudioSettingsModel model)
        {
            _model = model;
            // Реализация для вкладки звука
        }
    }
}