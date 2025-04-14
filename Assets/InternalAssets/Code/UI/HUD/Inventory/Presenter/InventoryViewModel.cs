using System;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.Core;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    public class InventoryViewModel : LayerViewModel
    {
        // Реактивное свойство для выбранного объекта
        private ReactiveProperty<ENetworkObjectType> _selectedObjectID = new ReactiveProperty<ENetworkObjectType>();
        public ReadOnlyReactiveProperty<ENetworkObjectType> SelectedObjectID => _selectedObjectID;
        
        private LocalInventorySession _localInventorySession;

        public InventoryViewModel(LocalInventorySession localInventorySession)
        {
            _localInventorySession = localInventorySession;
            
            // Подписываемся на изменение выбранного объекта
            _selectedObjectID
                .Where(objectID => objectID != ENetworkObjectType.None)
                .Subscribe(objectID => 
                {
                    _localInventorySession.CurrentSpawnObjectID = objectID;
                    
                    // Скрываем инвентарь
                    HideLayerItself();
                })
                .AddTo(_disposables);
        }
        
        public void SelectInventorySlot(ENetworkObjectType objectID)
        {
            _selectedObjectID.Value = objectID;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _selectedObjectID.Dispose();
        }
    }
}