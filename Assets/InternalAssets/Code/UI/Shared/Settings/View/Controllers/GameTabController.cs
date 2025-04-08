using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    public class GameTabController : UIToolkitElementView
    {
        private GameSettingsModel _model;
    
        public GameTabController(VisualElement root) : base(root) { }
    
        public void Bind(GameSettingsModel model)
        {
            _model = model;
            // Реализация для вкладки игры
        }
    }
}