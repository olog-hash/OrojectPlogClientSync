using K4os.Compression.LZ4;
using LiteNetLib.Utils;
using UnityEngine;

namespace ProjectOlog.Code.Network.Infrastructure.Core.Compressors
{
    /// <summary>
    /// Утилитный класс для сжатия и распаковки пакетов данных с использованием LZ4
    /// </summary>
    public static class LZ4PackageCompressor
    {
        /// <summary>
        /// Сжимает пакет данных для отправки
        /// </summary>
        /// <param name="dataPackage">Исходный пакет данных</param>
        /// <param name="compressionLevel">Уровень сжатия (по умолчанию быстрый)</param>
        /// <returns>Сжатый пакет, готовый к отправке</returns>
        public static NetDataPackage CompressPackage(NetDataPackage dataPackage,
            LZ4Level compressionLevel = LZ4Level.L00_FAST)
        {
            // Получаем исходные данные
            byte[] sourceData = dataPackage.Data;
            int sourceSize = sourceData.Length;

            // Создаем буфер для сжатых данных
            byte[] compressedBuffer = new byte[LZ4Codec.MaximumOutputSize(sourceSize)];

            // Сжимаем данные
            int compressedLength = LZ4Codec.Encode(
                sourceData, 0, sourceSize,
                compressedBuffer, 0, compressedBuffer.Length,
                compressionLevel);

            // Обрезаем буфер до фактического размера сжатых данных
            byte[] compressedData = new byte[compressedLength];
            System.Array.Copy(compressedBuffer, 0, compressedData, 0, compressedLength);

            // Создаем транспортный пакет с информацией для восстановления
            NetDataPackage transportPackage = new NetDataPackage(sourceSize, compressedData);

            // Логирование для отладки (опционально)
            //Debug.Log($"[LZ4] Пакет сжат: {sourceSize} → {compressedLength} байт (x{(float)sourceSize / compressedLength:F2})");

            return transportPackage;
        }

        /// <summary>
        /// Распаковывает полученный сжатый пакет
        /// </summary>
        /// <param name="compressedPackage">Сжатый транспортный пакет</param>
        /// <returns>Распакованный исходный пакет</returns>
        public static NetDataPackage DecompressPackage(NetDataPackage compressedPackage)
        {
            // Извлекаем данные из транспортного пакета
            int originalSize = compressedPackage.GetInt(); // Исходный размер
            byte[] compressedData = compressedPackage.GetByteArray(); // Сжатые данные

            // Создаем буфер для распакованных данных
            byte[] decompressedData = new byte[originalSize];

            // Распаковываем данные
            int decodedBytes = LZ4Codec.Decode(
                compressedData, 0, compressedData.Length,
                decompressedData, 0, decompressedData.Length);

            // Проверка успешности распаковки
            if (decodedBytes != originalSize)
            {
                Debug.LogError($"[LZ4] Ошибка распаковки: ожидалось {originalSize} байт, получено {decodedBytes}");
                return null; // Возвращаем null при ошибке
            }

            // Создаем пакет из распакованных данных
            NetDataPackage resultPackage = new NetDataPackage();
            resultPackage.FromData(decompressedData);

            return resultPackage;
        }
        
        /// <summary>
        /// Сжимает пакет данных для отправки
        /// </summary>
        /// <param name="dataPackage">Исходный пакет данных</param>
        /// <param name="compressionLevel">Уровень сжатия (по умолчанию быстрый)</param>
        /// <returns>Сжатый пакет, готовый к отправке</returns>
        public static HeadLessDataPacket CompressPackage(HeadLessDataPacket dataPackage,
            LZ4Level compressionLevel = LZ4Level.L00_FAST)
        {
            // Получаем исходные данные
            byte[] sourceData = dataPackage.Data;
            int sourceSize = sourceData.Length;

            // Создаем буфер для сжатых данных
            byte[] compressedBuffer = new byte[LZ4Codec.MaximumOutputSize(sourceSize)];

            // Сжимаем данные
            int compressedLength = LZ4Codec.Encode(
                sourceData, 0, sourceSize,
                compressedBuffer, 0, compressedBuffer.Length,
                compressionLevel);

            // Обрезаем буфер до фактического размера сжатых данных
            byte[] compressedData = new byte[compressedLength];
            System.Array.Copy(compressedBuffer, 0, compressedData, 0, compressedLength);

            // Создаем транспортный пакет с информацией для восстановления
            HeadLessDataPacket transportPackage = new HeadLessDataPacket(sourceSize, compressedData);

            // Логирование для отладки (опционально)
            //Debug.Log($"[LZ4] Пакет сжат: {sourceSize} → {compressedLength} байт (x{(float)sourceSize / compressedLength:F2})");

            return transportPackage;
        }

        /// <summary>
        /// Распаковывает полученный сжатый пакет
        /// </summary>
        /// <param name="compressedPackage">Сжатый транспортный пакет</param>
        /// <returns>Распакованный исходный пакет</returns>
        public static HeadLessDataPacket DecompressPackage(HeadLessDataPacket compressedPackage)
        {
            // Извлекаем данные из транспортного пакета
            int originalSize = compressedPackage.GetInt(); // Исходный размер
            byte[] compressedData = compressedPackage.GetByteArray(); // Сжатые данные

            // Создаем буфер для распакованных данных
            byte[] decompressedData = new byte[originalSize];

            // Распаковываем данные
            int decodedBytes = LZ4Codec.Decode(
                compressedData, 0, compressedData.Length,
                decompressedData, 0, decompressedData.Length);

            // Проверка успешности распаковки
            if (decodedBytes != originalSize)
            {
                Debug.LogError($"[LZ4] Ошибка распаковки: ожидалось {originalSize} байт, получено {decodedBytes}");
                return null; // Возвращаем null при ошибке
            }

            // Создаем пакет из распакованных данных
            HeadLessDataPacket resultPackage = new HeadLessDataPacket();
            resultPackage.FromData(decompressedData);

            return resultPackage;
        }
    }
}