using System;
using UnityEngine;

namespace ProjectOlog.Code.Network.Profiles.Snapshots.Core
{
    public class DeltaConverter
    {
        private const float PRECISION = 0.001f; // Точность до сотых
        private const float MAX_RANGE = 32.00f; // Максимальное значение

        // Конвертация float дельты в short
        public static short FloatDeltaToShort(float delta)
        {
            // Обрезаем дельту до допустимого диапазона
            delta = Mathf.Clamp(delta, -MAX_RANGE, MAX_RANGE);
        
            // Округляем до заданной точности и конвертируем в short
            return (short)Math.Round(delta / PRECISION);
        }

        // Конвертация short обратно в float дельту
        public static float ShortDeltaToFloat(short packed)
        {
            return packed * PRECISION;
        }
        
        // Важное дополнение - нормализация итоговой позиции
        public static float NormalizePoint(float position)
        {
            return (float)Math.Round(position / PRECISION) * PRECISION;
        }
        
        // Нормализация итогового вектора
        public static Vector3 NormalizeVector2(Vector2 vector)
        {
            return new Vector3(NormalizePoint(vector.x), NormalizePoint(vector.y));
        }
        
        // Важное дополнение - нормализация итоговой позиции
        public static Vector3 NormalizeVector3(Vector3 position)
        {
            return new Vector3(NormalizePoint(position.x), NormalizePoint(position.y),
                NormalizePoint(position.z));
        }
        
        // Проверка что отдельная компонента в границах
        public static bool IsComponentInDeltaLimit(float delta)
        {
            return Mathf.Abs(delta) <= MAX_RANGE;
        }

        public static bool IsVectorInDeltaLimit(Vector3 vector)
        {
            return IsComponentInDeltaLimit(vector.x) && IsComponentInDeltaLimit(vector.y) &&
                   IsComponentInDeltaLimit(vector.z);
        }
    }
}