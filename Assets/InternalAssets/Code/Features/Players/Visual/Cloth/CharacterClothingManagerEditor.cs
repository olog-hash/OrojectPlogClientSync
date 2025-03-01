using UnityEditor;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Visual.Cloth
{
#if UNITY_EDITOR
    
     [CustomEditor(typeof(CharacterClothingManager))]
    public class CharacterClothingManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterClothingManager manager = (CharacterClothingManager)target;
            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Refresh Clothing Cache"))
            {
                manager.CacheAllClothingItems();
            }

            if (GUILayout.Button("Randomize Outfit"))
            {
                manager.RandomizeOutfit();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Clothing Slots", EditorStyles.boldLabel);

            DrawClothingSlot("Boots", manager, manager.bootsSlot);
            DrawClothingSlot("Gloves", manager, manager.glovesSlot);
            DrawClothingSlot("Hat", manager, manager.hatSlot);
            DrawClothingSlot("Head", manager, manager.headSlot);
            DrawClothingSlot("Mask", manager, manager.maskSlot);
            DrawClothingSlot("Pants", manager, manager.pantsSlot);
            DrawClothingSlot("Shirt", manager, manager.shirtSlot);
            DrawClothingSlot("Backpack", manager, manager.backpackSlot);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawClothingSlot(string label, CharacterClothingManager manager, CharacterClothingManager.ClothingSlot slot)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(80));

            int currentIndex = slot.availableItems.IndexOf(slot.currentItem);
            int newIndex = EditorGUILayout.Popup(currentIndex, slot.availableItems.ToArray());
        
            if (newIndex != currentIndex && newIndex >= 0 && newIndex < slot.availableItems.Count)
            {
                string newItem = slot.availableItems[newIndex];
                // Используем метод менеджера для переключения предметов
                manager.EquipItemInSlot(label, newItem);
                
                // Обновляем сцену
                EditorUtility.SetDirty(manager);
                if (!Application.isPlaying)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    
#endif
}