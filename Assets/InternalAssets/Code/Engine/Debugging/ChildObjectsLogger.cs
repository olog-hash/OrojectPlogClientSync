using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Debugging
{
    public class ChildObjectsLogger : MonoBehaviour
    {
        private void Start()
        {
            LogChildObjects();
        }

        [Button("Вывести список дочерних обьектов")]
        public void LogChildObjects()
        {
            string result = "Child objects:\n";
   
            foreach (UnityEngine.Transform child in transform)
            {
                result += $"{child.name}\n";
            }

            Debug.Log(result);
        }
    }
}