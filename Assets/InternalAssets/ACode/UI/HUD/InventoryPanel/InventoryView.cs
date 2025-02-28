using System.Collections.Generic;
using ProjectOlog.Code.Core.Enums;
using ProjectOlog.Code.UI.Core;
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
            OnShowHideChanged(false);
            
            _currentViewModel = model;
            _currentViewModel.OnShowHideChanged += OnShowHideChanged;

            CreateInventorySlots();
        }

        protected override void OnUnbind(InventoryViewModel model)
        {
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;

            OnDestroy();
        }
        
        private void CreateInventorySlots()
        {
            foreach (var item in _inventoryItems)
            {
                // Создаем клон префаба слота инвентаря
                InventorySlotUI inventorySlot = Instantiate(_inventorySlotPrefab, _inventorySlotsParent);
                inventorySlot.gameObject.SetActive(true);

                // Настройка UI элементов внутри слота исходя из данных ObjectInventorySlot
                inventorySlot.Icon.sprite = item.Icon;
                inventorySlot.ItemName = item.ItemName;
                inventorySlot.Discription = item.Discription;
                inventorySlot.ObjectID = item.ObjectID;

                inventorySlot.SelectInventorySlot += OnInventorySlotClicked;

                _inventorySlotUI.Add(inventorySlot);
            }
        }

        public void OnInventorySlotClicked(ENetworkObjectType objectID)
        {
            if (this.isActiveAndEnabled)
            {
                _currentViewModel.OnInventorySlotClicked(objectID);
            }
        }

        private void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }
        
        public void OnDestroy()
        {
            foreach (var item in _inventorySlotUI)
            {
                item.SelectInventorySlot -= OnInventorySlotClicked;
            }
        }
    }
}