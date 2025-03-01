using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Visual.Cloth
{
    public class CharacterClothingManager : MonoBehaviour
    {
        [System.Serializable]
        public class ClothingSlot
        {
            public string currentItem;
            public List<string> availableItems = new List<string>();
        }

        // Слоты одежды для инспектора
        public ClothingSlot bootsSlot = new ClothingSlot();
        public ClothingSlot glovesSlot = new ClothingSlot();
        public ClothingSlot hatSlot = new ClothingSlot();
        public ClothingSlot headSlot = new ClothingSlot();
        public ClothingSlot maskSlot = new ClothingSlot();
        public ClothingSlot pantsSlot = new ClothingSlot();
        public ClothingSlot shirtSlot = new ClothingSlot();
        public ClothingSlot backpackSlot = new ClothingSlot();

        // Словари для хранения объектов
        public Dictionary<string, List<GameObject>> bootsItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> glovesItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> hatsItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> headsItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> masksItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> pantsItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> shirtsItems = new Dictionary<string, List<GameObject>>();
        public Dictionary<string, List<GameObject>> backpacksItems = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            CacheAllClothingItems();
        }

        public void CacheAllClothingItems()
        {
            ClearAllDictionaries();

            var allChildren = GetComponentsInChildren<Transform>(true);
            foreach (var child in allChildren)
            {
                string objName = child.gameObject.name;
                string baseName = GetBaseName(objName);

                if (objName.StartsWith("Boots_"))
                    AddToDict(bootsItems, baseName, child.gameObject, bootsSlot);
                else if (objName.StartsWith("Gloves_"))
                    AddToDict(glovesItems, baseName, child.gameObject, glovesSlot);
                else if (objName.StartsWith("Hats_"))
                    AddToDict(hatsItems, baseName, child.gameObject, hatSlot);
                else if (objName.StartsWith("Heads_"))
                    AddToDict(headsItems, baseName, child.gameObject, headSlot);
                else if (objName.StartsWith("Masks_"))
                    AddToDict(masksItems, baseName, child.gameObject, maskSlot);
                else if (objName.StartsWith("Pants_"))
                    AddToDict(pantsItems, baseName, child.gameObject, pantsSlot);
                else if (objName.StartsWith("Shirts_"))
                    AddToDict(shirtsItems, baseName, child.gameObject, shirtSlot);
                else if (objName.StartsWith("Backpacks_"))
                    AddToDict(backpacksItems, baseName, child.gameObject, backpackSlot);
            }

            DeactivateAllItems();
        }

        private void ClearAllDictionaries()
        {
            bootsItems.Clear();
            bootsSlot.availableItems.Clear();
            glovesItems.Clear();
            glovesSlot.availableItems.Clear();
            hatsItems.Clear();
            hatSlot.availableItems.Clear();
            headsItems.Clear();
            headSlot.availableItems.Clear();
            masksItems.Clear();
            maskSlot.availableItems.Clear();
            pantsItems.Clear();
            pantsSlot.availableItems.Clear();
            shirtsItems.Clear();
            shirtSlot.availableItems.Clear();
            backpacksItems.Clear();
            backpackSlot.availableItems.Clear();
        }

        public void RandomizeOutfit()
        {
            if (bootsSlot.availableItems.Count > 0)
                EquipItem(bootsItems, bootsSlot.availableItems[Random.Range(0, bootsSlot.availableItems.Count)],
                    ref bootsSlot.currentItem);
            if (glovesSlot.availableItems.Count > 0)
                EquipItem(glovesItems, glovesSlot.availableItems[Random.Range(0, glovesSlot.availableItems.Count)],
                    ref glovesSlot.currentItem);
            if (hatSlot.availableItems.Count > 0)
                EquipItem(hatsItems, hatSlot.availableItems[Random.Range(0, hatSlot.availableItems.Count)],
                    ref hatSlot.currentItem);
            if (headSlot.availableItems.Count > 0)
                EquipItem(headsItems, headSlot.availableItems[Random.Range(0, headSlot.availableItems.Count)],
                    ref headSlot.currentItem);
            if (maskSlot.availableItems.Count > 0)
                EquipItem(masksItems, maskSlot.availableItems[Random.Range(0, maskSlot.availableItems.Count)],
                    ref maskSlot.currentItem);
            if (pantsSlot.availableItems.Count > 0)
                EquipItem(pantsItems, pantsSlot.availableItems[Random.Range(0, pantsSlot.availableItems.Count)],
                    ref pantsSlot.currentItem);
            if (shirtSlot.availableItems.Count > 0)
                EquipItem(shirtsItems, shirtSlot.availableItems[Random.Range(0, shirtSlot.availableItems.Count)],
                    ref shirtSlot.currentItem);
            if (backpackSlot.availableItems.Count > 0)
                EquipItem(backpacksItems,
                    backpackSlot.availableItems[Random.Range(0, backpackSlot.availableItems.Count)],
                    ref backpackSlot.currentItem);
        }
        
        public void EquipItemInSlot(string slotType, string itemName)
        {
            switch (slotType)
            {
                case "Boots":
                    EquipItem(bootsItems, itemName, ref bootsSlot.currentItem);
                    break;
                case "Gloves":
                    EquipItem(glovesItems, itemName, ref glovesSlot.currentItem);
                    break;
                case "Hat":
                    EquipItem(hatsItems, itemName, ref hatSlot.currentItem);
                    break;
                case "Head":
                    EquipItem(headsItems, itemName, ref headSlot.currentItem);
                    break;
                case "Mask":
                    EquipItem(masksItems, itemName, ref maskSlot.currentItem);
                    break;
                case "Pants":
                    EquipItem(pantsItems, itemName, ref pantsSlot.currentItem);
                    break;
                case "Shirt":
                    EquipItem(shirtsItems, itemName, ref shirtSlot.currentItem);
                    break;
                case "Backpack":
                    EquipItem(backpacksItems, itemName, ref backpackSlot.currentItem);
                    break;
            }
        }

        // Остальные вспомогательные методы...
        private string GetBaseName(string fullName)
        {
            return string.Join("_", fullName.Split('_').Take(2));
        }

        private void AddToDict(Dictionary<string, List<GameObject>> dict, string baseName, GameObject obj,
            ClothingSlot slot)
        {
            if (!dict.ContainsKey(baseName))
            {
                dict[baseName] = new List<GameObject>();
                slot.availableItems.Add(baseName);
            }

            dict[baseName].Add(obj);
        }

        private void DeactivateAllItems()
        {
            foreach (var dict in new[]
                     {
                         bootsItems, glovesItems, hatsItems, headsItems,
                         masksItems, pantsItems, shirtsItems, backpacksItems
                     })
            {
                foreach (var items in dict.Values)
                {
                    foreach (var item in items)
                    {
                        item.SetActive(false);
                    }
                }
            }
        }

        private void EquipItem(Dictionary<string, List<GameObject>> itemDict, string itemName, ref string currentItem)
        {
            // Сначала деактивируем все предметы в этом словаре
            foreach (var items in itemDict.Values)
            {
                foreach (var item in items)
                {
                    if(item != null)
                        item.SetActive(false);
                }
            }

            // Теперь активируем только нужный предмет
            if (!string.IsNullOrEmpty(itemName) && itemDict.ContainsKey(itemName))
            {
                foreach (var obj in itemDict[itemName])
                {
                    if(obj != null)
                        obj.SetActive(true);
                }
                currentItem = itemName;
            }
        }
    }
}