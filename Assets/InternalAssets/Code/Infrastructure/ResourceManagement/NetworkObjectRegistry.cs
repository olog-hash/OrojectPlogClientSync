using System.Collections.Generic;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.ResourceManagement
{
    public static class NetworkObjectRegistry
    {
        private static Dictionary<short, NetworkObjectInfo> _registeredObjects = new Dictionary<short, NetworkObjectInfo>();

        public static void Reset()
        {
            _registeredObjects.Clear();
        }
        
        public static void RegisterNetworkObject(ENetworkObjectType id, string prefabName, string displayName, string description)
        {
            GameObject prefab = PrefabsResourcesLoader.Load(prefabName);
            if (prefab != null)
            {
                NetworkObjectInfo objectInfo = new NetworkObjectInfo(id, prefabName, displayName, description);
                _registeredObjects.Add((short)id, objectInfo);
            }
        }
        
        public static void RegisterNetworkObject(ENetworkObjectType id, string prefabName)
        {
            GameObject prefab = PrefabsResourcesLoader.Load(prefabName);
            if (prefab != null)
            {
                NetworkObjectInfo objectInfo = new NetworkObjectInfo(id, prefabName);
                _registeredObjects.Add((short)id, objectInfo);
            }
        }

        public static NetworkObjectInfo GetNetworkObjectInfo(ENetworkObjectType id)
        {
            if (_registeredObjects.TryGetValue((short)id, out NetworkObjectInfo objectInfo))
            {
                return objectInfo;
            }
            else
            {
                return null;
            }
        }

        public static GameObject GetNetworkObjectPrefab(ENetworkObjectType id)
        {
            NetworkObjectInfo objectInfo = GetNetworkObjectInfo(id);
            if (objectInfo != null)
            {
                return PrefabsResourcesLoader.Load(objectInfo.PrefabName);
            }
            else
            {
                Debug.LogWarning($"Network object with ID {id} not found. Returning ERROR prefab.");
                return PrefabsResourcesLoader.LoadError();
            }
        }
    }
    
    public class NetworkObjectInfo
    {
        public ENetworkObjectType Id { get; }
        public string PrefabName { get; }
        public string DisplayName { get; }
        public string Description { get; }

        public NetworkObjectInfo(ENetworkObjectType id, string prefabName, string displayName, string description)
        {
            Id = id;
            PrefabName = prefabName;
            DisplayName = displayName;
            Description = description;
        }
        
        public NetworkObjectInfo(ENetworkObjectType id, string prefabName)
        {
            Id = id;
            PrefabName = prefabName;
        }
    }
}