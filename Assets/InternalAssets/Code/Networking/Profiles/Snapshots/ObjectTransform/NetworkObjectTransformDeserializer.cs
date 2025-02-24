using NetCode;
using ProjectOlog.Code.Networking.Profiles.Snapshots.Core;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots.ObjectTransform
{
    
    public class NetworkObjectTransformDeserializer
    {
        private readonly BitReader _bitReader = new BitReader();

        public NetworkObjectTransform Deserialize(NetworkObjectTransform before, byte[] array)
        {
            NetworkObjectTransform networkObjectTransform = new();
            
            _bitReader.SetArray(array);
            
            bool isGlobal = _bitReader.ReadBool();
            
            if (isGlobal)
            {
                networkObjectTransform = DeserializeGlobal();
            }
            else
            {
                networkObjectTransform = DeserializeChanged(before);
            }
            
            
            return networkObjectTransform; 
        }

        private NetworkObjectTransform DeserializeGlobal()
        {
            NetworkObjectTransform result = new();
            
            // Синхронизируем позицию
            bool isPositionSync = _bitReader.ReadBool();
            if (isPositionSync)
            {
                var position = new Vector3(
                    _bitReader.ReadFloat(),
                    _bitReader.ReadFloat(),
                    _bitReader.ReadFloat()
                );

                result.Position = DeltaConverter.NormalizeVector3(position);
            }
            
            // Синхронизируем вращение
            bool isRotationSync = _bitReader.ReadBool();

            if (isRotationSync)
            {
                var rotation = new Vector3(
                    _bitReader.ReadFloat(NetworkTransformLimits.Rotation),
                    _bitReader.ReadFloat(NetworkTransformLimits.Rotation),
                    _bitReader.ReadFloat(NetworkTransformLimits.Rotation)
                );

                result.Rotation = rotation;
            }
            
            // Синхронизируем размер
            bool isScaleSync = _bitReader.ReadBool();
            if (isScaleSync)
            {
                var scale = new Vector3(
                    _bitReader.ReadFloat(),
                    _bitReader.ReadFloat(),
                    _bitReader.ReadFloat()
                );

                result.Scale = DeltaConverter.NormalizeVector3(scale);
            }
            
            _bitReader.Reset();
            return result;
        }
        
        private NetworkObjectTransform DeserializeChanged(NetworkObjectTransform before)
        {
            NetworkObjectTransform result = new();
            
            // Синхронизируем позицию
            bool isPositionSync = _bitReader.ReadBool();
            if (isPositionSync)
            {
                // Проверяем, чтобы в прошлом снимке были данные (для изменения)!
                bool isBaselineHadPosition = _bitReader.ReadBool();
                if (isBaselineHadPosition)
                {
                    // Используем ли мы расширенную позицию (полную) или достаточно дельты
                    bool isPositionExtended = _bitReader.ReadBool();
                    if (isPositionExtended)
                    {
                        // Если дельта превосходит лимит в +-31 м, тогда это полная позицию.
                        var position = new Vector3(
                            _bitReader.ReadFloat(before.Position.Value.x),
                            _bitReader.ReadFloat(before.Position.Value.y),
                            _bitReader.ReadFloat(before.Position.Value.z)
                        );
                        
                        result.Position = DeltaConverter.NormalizeVector3(position);
                    }
                    else
                    {
                        short zeroShort = 0;

                        float deltaX = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));
                        float deltaY = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));
                        float deltaZ = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));

                        result.Position = new Vector3(
                            before.Position.Value.x + deltaX,
                            before.Position.Value.y + deltaY,
                            before.Position.Value.z + deltaZ
                        );
                    }
                }
                else
                {
                    // В таком случаи нам полностью отправили позицию
                    var position = new Vector3(
                        _bitReader.ReadFloat(),
                        _bitReader.ReadFloat(),
                        _bitReader.ReadFloat()
                    );

                    result.Position = DeltaConverter.NormalizeVector3(position);
                }
            }
            
            // Синхронизируем вращение
            bool isRotationSync = _bitReader.ReadBool();
            if (isRotationSync)
            {
                // Проверяем, чтобы в прошлом снимке были данные (для изменения)!
                bool isBaselineHadRotation = _bitReader.ReadBool();
                if (isBaselineHadRotation)
                {
                    // Записываем только измененные значения
                    float rotationX = _bitReader.ReadFloat(before.Rotation.Value.x, NetworkTransformLimits.Rotation);
                    float rotationY = _bitReader.ReadFloat(before.Rotation.Value.y, NetworkTransformLimits.Rotation);
                    float rotationZ = _bitReader.ReadFloat(before.Rotation.Value.z, NetworkTransformLimits.Rotation);

                    result.Rotation = new Vector3(rotationX, rotationY, rotationZ);
                }
                else
                {
                    // Полностью получаем данные
                    float rotationX = _bitReader.ReadFloat(NetworkTransformLimits.Rotation);
                    float rotationY = _bitReader.ReadFloat(NetworkTransformLimits.Rotation);
                    float rotationZ = _bitReader.ReadFloat(NetworkTransformLimits.Rotation);

                    result.Rotation = new Vector3(rotationX, rotationY, rotationZ);
                }
            }
            
            // Синхронизируем размер
            bool isScaleSync = _bitReader.ReadBool();
            if (isScaleSync)
            {
                // Проверяем, чтобы в прошлом снимке были данные (для изменения)!
                bool isBaselineHadScale = _bitReader.ReadBool();
                if (isBaselineHadScale)
                {
                    // Записываем только измененные значения
                    var scale = new Vector3(
                        _bitReader.ReadFloat(before.Scale.Value.x),
                        _bitReader.ReadFloat(before.Scale.Value.y), 
                        _bitReader.ReadFloat(before.Scale.Value.z)
                    );

                    result.Scale = DeltaConverter.NormalizeVector3(scale);
                }
                else
                {
                    // Тогда полностью отправляем размер
                    var scale = new Vector3(
                        _bitReader.ReadFloat(),
                        _bitReader.ReadFloat(), 
                        _bitReader.ReadFloat()
                    );

                    result.Scale = DeltaConverter.NormalizeVector3(scale);
                }
            }
            
            
            _bitReader.Reset();
            return result;
        }
    }
}