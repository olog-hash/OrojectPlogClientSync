using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Client;
using ProjectOlog.Code.Network.Infrastructure.Core.Batch;
using UnityEngine;

namespace ProjectOlog.Code.Network.Infrastructure.Core
{
    public class NetTransportProvider
    {
        private const bool IS_BATCH_ENABLE = true;

        private BatchContainer _batchContainer;
        private IClientSender _clientSender;

        public NetTransportProvider(IClientSender clientSender)
        {
            _batchContainer = new BatchContainer();
            _clientSender = clientSender;
            
            Debug.Log($"Бранч работает в режиме {IS_BATCH_ENABLE}");
        }

        /// <summary>
        /// Добавляет сетевой пакет в очередь на отправку.
        /// Если батчинг отключен - отправляет пакет немедленно.
        /// </summary>
        public void EnqueuePackage(PackageHeader header, NetDataPackage sourcePackage)
        {
            if (sourcePackage is null) return;

            if (IS_BATCH_ENABLE)
            {
                _batchContainer.AddPackage(header, sourcePackage);
            }
            else
            {
                TransmitPackage(header, sourcePackage);
            }
        }

        /// <summary>
        /// Обрабатывает и отправляет все накопленные сетевые пакеты.
        /// Вызывается в конце цикла обновления сервера, обычно в LateUpdate (чтобы попали одно-кадровые и тикрейтные пакеты).
        /// </summary>
        public void ProcessBatchQueue()
        {
            var batchPackets = _batchContainer.GetBatchPackages();

            for (int i = 0; i < batchPackets.Length; i++)
            {
                if (batchPackets[i].PackageCount == 0) continue;

                // Если в батче только один пакет, отправляем его напрямую
                if (batchPackets[i].PackageCount == 1)
                {
                    var singlePackage = batchPackets[i].Serialize()[0];
                    TransmitPackage(batchPackets[i].PackageHeader, singlePackage);
                }
                else
                {
                    Debug.Log("МНОГО ПАКЕТов");
                    var dataPackageArray = batchPackets[i].Serialize();
                    TransmitBatchedPackages(batchPackets[i].PackageHeader, dataPackageArray);
                }
            }

            _batchContainer.Clear();
        }

        // Отправляет одиночный пакет через сеть
        private void TransmitPackage(PackageHeader header, NetDataPackage sourcePackage)
        {
            var dataPackage = new NetDataPackage(header.NetWorkerName, header.MethodName, sourcePackage);
            TransmitToNetwork(header, dataPackage);
        }

        // Отправляет массив пакетов через сеть
        private void TransmitBatchedPackages(PackageHeader header, NetDataPackage[] packageArray)
        {
            var dataPackage = new NetDataPackage(header.NetWorkerName, header.MethodName, packageArray);
            TransmitToNetwork(header, dataPackage);
        }

        // Выполняет непосредственную отправку данных в сеть с учетом типа отправки
        private void TransmitToNetwork(PackageHeader header, NetDataPackage dataPackage)
        {
            var serializedData = _clientSender.WritePacket(dataPackage.GetData());

            _clientSender.Send(serializedData, header.DeliveryMethod);
        }
    }
}