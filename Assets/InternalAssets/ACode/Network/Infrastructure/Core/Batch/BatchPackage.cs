using System.Collections.Generic;
using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Infrastructure.Core.Batch
{
    /// <summary>
    /// Обертка вокруг коллекции однотипных пакетов (с одинаковыми заголовками и отправной сигнатурой)
    /// </summary>
    public class BatchPackage
    {
        public const int MAX_SIZE = 1024; // 1kb лимит на batch

        public bool IsFull => _currentSize >= MAX_SIZE;
        public int PackageCount => _sourcePackages.Count;
        public PackageHeader PackageHeader => _packageHeader;
        
        private PackageHeader _packageHeader;
        private List<NetDataPackage> _sourcePackages;
        private int _currentSize;
        
        public BatchPackage(PackageHeader packageHeader)
        {
            _packageHeader = packageHeader;
            _sourcePackages = new List<NetDataPackage>();
            _currentSize = 0;
        }

        public bool TryAdd(NetDataPackage sourceDataPacket)
        {
            if (_sourcePackages.Count > 0 && ((_currentSize + sourceDataPacket.Length) > MAX_SIZE))
                return false;

            _sourcePackages.Add(sourceDataPacket);
            _currentSize += sourceDataPacket.Length;
            return true;
        }

        public NetDataPackage[] Serialize()
        {
            return _sourcePackages.ToArray();
        }

        public bool IsEqual(PackageHeader otherPacketHeader)
        {
            if (otherPacketHeader.NetWorkerName == null || _packageHeader.NetWorkerName == null ||
                otherPacketHeader.MethodName == null || _packageHeader.MethodName == null)
                return false;

            // Проверяем равенство строковых полей через string.Equals для большей надежности
            bool isStringsEqual = string.Equals(_packageHeader.NetWorkerName, otherPacketHeader.NetWorkerName) &&
                                  string.Equals(_packageHeader.MethodName, otherPacketHeader.MethodName);

            // Проверяем enum'ы
            bool isTypesEqual = _packageHeader.DeliveryMethod == otherPacketHeader.DeliveryMethod;

            return isStringsEqual && isTypesEqual;
        }
    }
}