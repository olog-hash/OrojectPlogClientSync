using System.Collections.Generic;
using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Infrastructure.Core.Batch
{
    /// <summary>
    /// Необходим для группировки пакетов с одинаковой сигнатурой (нетворкер, метод, тип отправки и т.д)
    /// </summary>
    public class BatchContainer
    {
        private List<BatchPackage> _batchPackages = new();
        private BatchPackage _lastAddedBatch; // Кэшируем последний использованный батч

        public BatchContainer()
        {
            ClearWithCapacity();
        }

        public void AddPackage(PackageHeader header, NetDataPackage sourceDataPacket)
        {
            // Сначала проверяем последний использованный батч - это наиболее вероятный кандидат
            if (_lastAddedBatch != null && !_lastAddedBatch.IsFull && _lastAddedBatch.IsEqual(header))
            {
                if (_lastAddedBatch.TryAdd(sourceDataPacket))
                    return;
            }

            // Проходим по списку только если не удалось использовать кэшированный батч
            for (int i = 0; i < _batchPackages.Count; i++)
            {
                var batch = _batchPackages[i];
                if (!batch.IsFull && batch.IsEqual(header))
                {
                    if (batch.TryAdd(sourceDataPacket))
                    {
                        _lastAddedBatch = batch;
                        return;
                    }
                }
            }

            // Создаем новый батч если не нашли подходящий
            var newBatch = new BatchPackage(header);
            newBatch.TryAdd(sourceDataPacket);
            _batchPackages.Add(newBatch);
            _lastAddedBatch = newBatch;
        }

        public BatchPackage[] GetBatchPackages()
        {
            return _batchPackages.ToArray();
        }
        
        public void Clear()
        {
            _batchPackages.Clear();
            _lastAddedBatch = null;
        }

        public void ClearWithCapacity()
        {
            _batchPackages = new List<BatchPackage>();
            _lastAddedBatch = null;
        }
    }
}