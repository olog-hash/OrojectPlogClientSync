using System;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    public class InventoryViewModel : BaseViewModel, ILayer
    {
        public Action OnHandleClose;
        public Action<bool> OnShowHideChanged;

        private LocalPlayerSession _localPlayerSession;

        public InventoryViewModel(LocalPlayerSession localPlayerSession)
        {
            _localPlayerSession = localPlayerSession;
            OnShowHideChanged = null;
        }
        
        public void OnInventorySlotClicked(ENetworkObjectType objectID)
        {
            _localPlayerSession.CurrentSpawnObjectID = objectID;
            
            OnHandleClose?.Invoke();
        }
        
        public void ShowLayer()
        {
            OnShowHideChanged?.Invoke(true);
        }

        public void HideLayer()
        {
            OnShowHideChanged?.Invoke(false);
        }
    }
}