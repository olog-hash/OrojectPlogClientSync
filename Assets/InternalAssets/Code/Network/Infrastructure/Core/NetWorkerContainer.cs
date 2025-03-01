using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteNetLib;
using LiteNetLib.Utils;
using UnityEngine;
using PrimitiveType = LiteNetLib.Utils.PrimitiveType;

namespace ProjectOlog.Code.Network.Infrastructure.Core
{
    public class NetWorkerContainer
    {
        public NetWorkerClient NetWorker { get; }
        private Dictionary<string, MethodInfo> _cachedMethods;

        public NetWorkerContainer(NetWorkerClient netWorker)
        {
            NetWorker = netWorker;
            
            CacheNetworkCallbackMethods();
        }

        private void CacheNetworkCallbackMethods()
        {
            _cachedMethods = NetWorker.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<NetworkCallback>() != null)
                .ToDictionary(m => m.Name, m => m);
        }
        
        public void Initialize()
        {
            NetWorker.Initialize();
        }

        /// <summary>
        /// Распаковывает полученный пакет сервера для последующей отправки в адресованный нетворкер
        /// </summary>
        public void HandleRequest(NetPeer peer, NetDataPackage dataPackage)
        {
            if (dataPackage.CurrentType != PrimitiveType.String) return;
            string methodName = dataPackage.GetString();

            // Проверяем следующий тип - это либо Package либо PackagesArray
            if (dataPackage.CurrentType == PrimitiveType.Package)
            {
                var sourceDataPackage = dataPackage.GetPackage();
                ExecuteMethod(peer, methodName, sourceDataPackage);
            }
            else if (dataPackage.CurrentType == PrimitiveType.PackagesArray)
            {
                var packages = dataPackage.GetPackagesArray();
                foreach (var sourceDataPackage in packages)
                {
                    ExecuteMethod(peer, methodName, sourceDataPackage);
                }
                
                Debug.Log($"Прибыло много пакетов типа {methodName} в кол-ве {packages.Length}");
            }
        }

        private void ExecuteMethod(NetPeer peer, string methodName, NetDataPackage sourceDataPackage)
        {
            // NetPeer peer, DataPackage dataPackage
            if (_cachedMethods.TryGetValue(methodName, out MethodInfo method))
            {
                if (NetWorker.IsAvaliableToExecute(peer))
                {
                    method.Invoke(NetWorker, new object[] { peer, sourceDataPackage });
                }
            }
            else
            {
                Console.WriteLine($"Method {methodName} not found or not marked with NetworkCallback attribute");
            }
        }

        public IEnumerable<string> GetAvailableMethods()
        {
            return _cachedMethods.Keys;
        }
    }
}