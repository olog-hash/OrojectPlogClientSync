using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View.Controllers
{
    // Контроллер для заголовка
    public class HeaderController : UIToolkitElementView
    {
        private Label _roomNameLabel;
        private Label _serverNameLabel;
        private Label _mapNameLabel;
        private Label _modeNameLabel;
    
        private TabViewModel _model;

        public HeaderController(VisualElement root) : base(root)
        {
            
        }
        
        protected override void SetVisualElements()
        {
            _roomNameLabel = Root.Q<Label>("room-name");
            _serverNameLabel = Root.Q<Label>("server-name");
            _mapNameLabel = Root.Q<Label>("map-name");
            _modeNameLabel = Root.Q<Label>("mode-name");
        }
    
        public void Bind(TabViewModel model)
        {
            _model = model;
        
            // Подписываемся на изменения
            _model.MatchInfoModel.RoomName
                .Subscribe(name => _roomNameLabel.text = name)
                .AddTo(_disposables);
            
            _model.MatchInfoModel.ServerName
                .Subscribe(name => _serverNameLabel.text = name)
                .AddTo(_disposables);
            
            _model.MatchInfoModel.MapName
                .Subscribe(name => _mapNameLabel.text = name)
                .AddTo(_disposables);
            
            _model.MatchInfoModel.ModeName
                .Subscribe(name => _modeNameLabel.text = name)
                .AddTo(_disposables);
        }
    
        public void Unbind()
        {
            _model = null;
            _disposables.Clear();
        }
    }
}