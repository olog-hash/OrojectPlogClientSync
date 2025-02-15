using System.Collections.Generic;
using ProjectOlog.Code.Core.Enums;
using UnityEngine;

namespace ProjectOlog.Code.Gameplay.Battle
{
    public class BattleContentFactory : MonoBehaviour
    {
        [Header("Characters")]
        public GameObject FirstPersonCharacter;
        public GameObject ThirdPersonCharacter;

        [Header("Stuff")]
        public GameObject ERROR_Object;

        [Header("Spawn objects")]
        [SerializeField]
        public List<SpawnObjectData> SpawnObjects = new List<SpawnObjectData>();

        public GameObject GetSpawnObject(ENetworkObjectType type)
        {
            for (int i = 0; i < SpawnObjects.Count; i++)
            {
                if (SpawnObjects[i].Type == type)
                {
                    return SpawnObjects[i].Object;
                }
            }

            return ERROR_Object;
        }
    }

    [System.Serializable]
    public class SpawnObjectData
    {
        public ENetworkObjectType Type;
        public GameObject Object;
    }
}