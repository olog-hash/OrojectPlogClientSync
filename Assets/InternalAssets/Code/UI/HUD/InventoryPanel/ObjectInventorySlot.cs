using System;
using ProjectOlog.Code.Core.Enums;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    [Serializable]
    public class ObjectInventorySlot
    {
        public Sprite Icon;
        public string ItemName;
        public string Discription;

        public ENetworkObjectType ObjectID;
    }
}