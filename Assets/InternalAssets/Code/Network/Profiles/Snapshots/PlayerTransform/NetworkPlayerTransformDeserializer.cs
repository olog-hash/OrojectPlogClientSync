using NetCode;
using ProjectOlog.Code.Network.Profiles.Snapshots.Core;
using UnityEngine;

namespace ProjectOlog.Code.Network.Profiles.Snapshots.PlayerTransform
{
    public class NetworkPlayerTransformDeserializer
    {
        private readonly BitReader _bitReader = new BitReader();

        public NetworkPlayerTransform Deserialize(NetworkPlayerTransform before, byte[] array)
        {
            NetworkPlayerTransform networkPlayerTransform = new();
            
            _bitReader.SetArray(array);
            
            bool isGlobal = _bitReader.ReadBool();
            
            if (isGlobal)
            {
                networkPlayerTransform = DeserializeGlobal();
            }
            else
            {
                networkPlayerTransform = DeserializeChanged(before);
            }
            
            
            return networkPlayerTransform; 
        }

        private NetworkPlayerTransform DeserializeGlobal()
        {
            NetworkPlayerTransform result = new();

            result.RemoteTime = _bitReader.ReadDouble();
            
            result.Position = new Vector3(
                _bitReader.ReadFloat(),
                _bitReader.ReadFloat(),
                _bitReader.ReadFloat()
            );
        
            result.Position = DeltaConverter.NormalizeVector3(result.Position);
            result.YawDegrees = _bitReader.ReadFloat(NetworkTransformLimits.Yaw);
            result.PitchDegrees = _bitReader.ReadFloat(NetworkTransformLimits.Pitch);
            result.IsGrounded = _bitReader.ReadBool();
            result.CharacterBodyState = _bitReader.ReadByte();
            
            _bitReader.Reset();
            
            return result;
        }
        
        private NetworkPlayerTransform DeserializeChanged(NetworkPlayerTransform before)
        {
            NetworkPlayerTransform result = new();
            
            result.RemoteTime = _bitReader.ReadDouble();
            
            bool isPositionExtended = _bitReader.ReadBool();
            
            if (isPositionExtended)
            {
                result.Position = new Vector3(
                    _bitReader.ReadFloat(before.Position.x),
                    _bitReader.ReadFloat(before.Position.y),
                    _bitReader.ReadFloat(before.Position.z)
                );
            }
            else
            {
                short zeroShort = 0;

                float deltaX = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));
                float deltaY = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));
                float deltaZ = DeltaConverter.ShortDeltaToFloat(_bitReader.ReadShort(zeroShort));

                result.Position = new Vector3(
                    before.Position.x + deltaX,
                    before.Position.y + deltaY,
                    before.Position.z + deltaZ
                );
            }

            result.Position = DeltaConverter.NormalizeVector3(result.Position);
            result.YawDegrees = _bitReader.ReadFloat(before.YawDegrees, NetworkTransformLimits.Yaw);
            result.PitchDegrees = _bitReader.ReadFloat(before.PitchDegrees, NetworkTransformLimits.Pitch);
            result.IsGrounded = _bitReader.ReadBool(); // Нет смысла записывать изменение - это bool
            result.CharacterBodyState = _bitReader.ReadByte(before.CharacterBodyState);

            _bitReader.Reset();
            
            return result;
        }
        
        private void LogDeltaDebugInfo(NetworkPlayerTransform baseline, NetworkPlayerTransform updated, Vector3 packedDelta)
        {
            Debug.Log($"(Клиент) Прошлая позиция: {baseline.Position}");
            Debug.Log($"(Клиент) Текущая позиция: {updated.Position}");
            Debug.Log($"(Сервер) Дельта: {packedDelta}");
        }
    }
}