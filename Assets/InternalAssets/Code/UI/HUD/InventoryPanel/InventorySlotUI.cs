using System;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image Icon;
        public string ItemName;
        public string Discription;
        
        public ENetworkObjectType ObjectID;

        public Action<ENetworkObjectType> SelectInventorySlot;

        public void OnClick()
        {
            SelectInventorySlot?.Invoke(ObjectID);
        }
    }
}