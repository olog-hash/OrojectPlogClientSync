using System.Collections.Generic;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.ResourceManagement
{
    public static class PrefabsResourcesLoader
    {
        private const string ERROR_PREFAB_NAME = "ERROR";
        
        private static Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
        
        public static void Reset()
        {
            _prefabs = new Dictionary<string, GameObject>();
        }
        
        public static void RegisterAllPrefabs(string folderPath)
        {
            // Загружаем все объекты в указанной директории и её поддиректориях
            Object[] objects = Resources.LoadAll(folderPath, typeof(GameObject));

            // Перебираем все загруженные объекты
            foreach (Object obj in objects)
            {
                if (obj is GameObject)
                {
                    var prefab = obj as GameObject;
                    
                    if(!_prefabs.ContainsKey(prefab.name))
                        _prefabs.Add(prefab.name, prefab);
                }
            }
            
            Debug.Log($"Prefabs loading was successful. {_prefabs.Count}");
        }

        public static GameObject Load(string prefabName)
        {
            if (_prefabs.ContainsKey(prefabName))
            {
                return _prefabs[prefabName];
            }
            else
            {
                Debug.LogWarning($"Prefab with name '{prefabName}' not found.");
                return LoadError();
            }
        }
        
        public static GameObject LoadError()
        {
            if (_prefabs.ContainsKey(ERROR_PREFAB_NAME))
            {
                return _prefabs[ERROR_PREFAB_NAME];
            }

            // Видимо ERROR обьекта нету.
            return null;
        }
    }
}
