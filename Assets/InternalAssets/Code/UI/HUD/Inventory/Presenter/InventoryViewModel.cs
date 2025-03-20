using System;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.Core;
using R3;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    public class InventoryViewModel : BaseViewModel, ILayer
    {
        // Реактивное свойство для выбранного объекта
        private ReactiveProperty<ENetworkObjectType> _selectedObjectID = new ReactiveProperty<ENetworkObjectType>();
        public ReadOnlyReactiveProperty<ENetworkObjectType> SelectedObjectID => _selectedObjectID;
        
        // Событие для закрытия, как в оригинальном коде
        public event Action OnHandleClose;
        
        private LocalPlayerSession _localPlayerSession;

        public InventoryViewModel(LocalPlayerSession localPlayerSession)
        {
            _localPlayerSession = localPlayerSession;
            
            // Подписываемся на изменение выбранного объекта
            _selectedObjectID
                .Where(objectID => objectID != ENetworkObjectType.None)
                .Subscribe(objectID => 
                {
                    _localPlayerSession.CurrentSpawnObjectID = objectID;
                    OnHandleClose?.Invoke();
                })
                .AddTo(_disposables);
        }
        
        public void SelectInventorySlot(ENetworkObjectType objectID)
        {
            _selectedObjectID.Value = objectID;
        }
        
        public void ShowLayer() => Show();

        public void HideLayer() => Hide();
        
        public override void Dispose()
        {
            base.Dispose();
            _selectedObjectID.Dispose();
        }
    }
}