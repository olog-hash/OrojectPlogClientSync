using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteNetLib.Utils;
using UnityEngine;
using PrimitiveType = LiteNetLib.Utils.PrimitiveType;

namespace ProjectOlog.Code.Engine.StateMachines.Interactables.Networking
{
    public class ObjectNetworkerContainer : IObjectNetworkerContainer
    {
        public ObjectNetworker ObjectNetworker { get; }
        private Dictionary<string, MethodInfo> _cachedMethods;

        public ObjectNetworkerContainer(ObjectNetworker netWorker)
        {
            netWorker.Init(this);
            ObjectNetworker = netWorker;
            
            CacheNetworkCallbackMethods();
        }

        private void CacheNetworkCallbackMethods()
        {
            _cachedMethods = ObjectNetworker.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<NetworkCallback>() != null)
                .ToDictionary(m => m.Name, m => m);
        }

        public void HandleRequest(NetDataPackage dataPackage)
        {
            if (dataPackage.CurrentType != PrimitiveType.String) return;
            string methodName = dataPackage.GetString();

            if (dataPackage.CurrentType != PrimitiveType.Package) return;
            var sourceDataPackage = dataPackage.GetPackage();
            
            // NetPeer peer, DataPackage dataPackage
            if (_cachedMethods.TryGetValue(methodName, out MethodInfo method))
            {
                method.Invoke(ObjectNetworker, new object[] { sourceDataPackage });
            }
            else
            {
                Debug.Log($"Method {methodName} not found or not marked with NetworkCallback attribute");
            }
        }

        public IEnumerable<string> GetAvailableMethods()
        {
            return _cachedMethods.Keys;
        }
    }
}