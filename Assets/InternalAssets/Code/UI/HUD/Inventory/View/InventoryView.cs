using System.Collections.Generic;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.Core;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    public class InventoryView: AbstractScreen<InventoryViewModel>
    {
        [SerializeField] private InventorySlotUI _inventorySlotPrefab;
        [SerializeField] private Transform _inventorySlotsParent; 
        [SerializeField] private List<ObjectInventorySlot> _inventoryItems = new List<ObjectInventorySlot>();
        
        private List<InventorySlotUI> _inventorySlotUI = new List<InventorySlotUI>();

        private InventoryViewModel _currentViewModel;
        
        protected override void OnBind(InventoryViewModel model)
        {
            // Установим HideOnAwake для скрытия экрана после инициализации
            HideOnAwake = true;
            
            CreateInventorySlots();
            
            // Подписываемся на изменения выбранного предмета
            model.SelectedObjectID
                .Subscribe(OnSelectedItemChanged)
                .AddTo(_disposables);
        }

        protected override void OnUnbind(InventoryViewModel model)
        {
            ClearInventorySlots();
        }
        
        private void OnSelectedItemChanged(ENetworkObjectType objectID)
        {
            // Можно добавить визуальное выделение выбранного слота
            foreach (var slot in _inventorySlotUI)
            {
                bool isSelected = slot.ObjectID == objectID;
            }
        }
        
        private void CreateInventorySlots()
        {
            ClearInventorySlots();
            
            foreach (var item in _inventoryItems)
            {
                // Создаем клон префаба слота инвентаря
                InventorySlotUI inventorySlot = Instantiate(_inventorySlotPrefab, _inventorySlotsParent);
                inventorySlot.gameObject.SetActive(true);

                // Настройка UI элементов внутри слота
                inventorySlot.Icon.sprite = item.Icon;
                inventorySlot.ItemName = item.ItemName;
                inventorySlot.Discription = item.Discription;
                inventorySlot.ObjectID = item.ObjectID;

                // Привязываем обработчик клика к методу в ViewModel
                inventorySlot.SelectInventorySlot = (objectID) => _model.SelectInventorySlot(objectID);

                _inventorySlotUI.Add(inventorySlot);
            }
        }
        
        private void ClearInventorySlots()
        {
            foreach (var item in _inventorySlotUI)
            {
                if (item != null)
                {
                    item.SelectInventorySlot = null;
                    Destroy(item.gameObject);
                }
            }
            
            _inventorySlotUI.Clear();
        }
    }
}