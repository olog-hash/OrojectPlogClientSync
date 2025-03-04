using LiteNetLib.Utils;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Network.Packets.SystemSync.Send
{
    /// <summary>
    /// Содержит данные о позиции, вращении и физическом состоянии игрока,
    /// отправляемые от клиента на сервер для синхронизации.
    /// </summary>
    public class PlayerTransformStateData : INetPackageSerializable
    {
        public ushort LastStateVersion;
        
        // Данные игрока
        public Vector3 Position;
        public float YawDegrees;
        public float PitchDegrees;

        // Остальное
        public float PreviousFallVelocity;
        public bool IsGrounded;
        public ECharacterBodyState CharacterBodyState;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(LastStateVersion, Position, YawDegrees, PitchDegrees, PreviousFallVelocity, IsGrounded,
                (byte)CharacterBodyState);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastStateVersion = dataPackage.GetUShort();
            
            Position = dataPackage.GetVector3();
            YawDegrees = dataPackage.GetFloat();
            PitchDegrees = dataPackage.GetFloat();
            PreviousFallVelocity = dataPackage.GetFloat();
            IsGrounded = dataPackage.GetBool();
            CharacterBodyState = (ECharacterBodyState)dataPackage.GetByte();
        }
    }
}