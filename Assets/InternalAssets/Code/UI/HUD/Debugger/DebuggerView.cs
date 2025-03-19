using ProjectOlog.Code.Network.Client;
using ProjectOlog.Code.UI.Core;
using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Debugger
{
    public class DebuggerView: AbstractScreen<DebuggerViewModel>
    {
        [SerializeField]
        private TextMeshProUGUI _debuggerText;
        
        [SerializeField]
        private TextMeshProUGUI _secondDebuggerText;
        
        // ViewModel
        private DebuggerViewModel _currentViewModel;
        
        protected override void OnBind(DebuggerViewModel model)
        {
            HideOnAwake = true;
            
            _currentViewModel = model;
            _currentViewModel.OnShowHideChanged += OnShowHideChanged;
            _currentViewModel.OnDebuggerUpdate += OnDebuggerUpdate;
        }
        
        protected override void OnUnbind(DebuggerViewModel model)
        {
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;
            _currentViewModel.OnDebuggerUpdate -= OnDebuggerUpdate;
        }

        private void Update()
        {
            _debuggerText.text = $@"Build Project_Olog 
PreAlpha ver 0.4\n
{_currentViewModel.FrameRate} fps ({NetworkTime.LastLocalTick} tickrate)
Pos: {_currentViewModel.Position}
Velocity: {_currentViewModel.Velocity}

C_ServerTick: {_currentViewModel.CurrentServerTick}
C_LocalTick: {_currentViewModel.CurrentLocalTick}
C_Users: {_currentViewModel.UsersCount}
C_Entities: {_currentViewModel.NetworkEntities}
Ping: {_currentViewModel.Ping}
IN: {_currentViewModel.BytesInPerSecond / 1000f} KB/s({_currentViewModel.PacketsInPerSecond})
OUT: {_currentViewModel.BytesOutPerSecond / 1000f} KB/s({_currentViewModel.PacketsOutPerSecond})
Loss: {_currentViewModel.PacketLoss}";

            _secondDebuggerText.text = string.Empty;
        }

        private void OnDebuggerUpdate()
        {
            
        }
        
        private void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }
    }
}