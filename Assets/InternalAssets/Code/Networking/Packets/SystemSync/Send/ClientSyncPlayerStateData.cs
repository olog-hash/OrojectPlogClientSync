using LiteNetLib.Utils;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Send
{
    public class ClientSyncPlayerStateData : INetPackageSerializable
    {
        // Пакет пустой или же нет
        public bool IsPacketAvaliable;

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
            return new NetDataPackage(Position, YawDegrees, PitchDegrees, PreviousFallVelocity, IsGrounded,
                (byte)CharacterBodyState);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            Position = dataPackage.GetVector3();
            YawDegrees = dataPackage.GetFloat();
            PitchDegrees = dataPackage.GetFloat();
            PreviousFallVelocity = dataPackage.GetFloat();
            IsGrounded = dataPackage.GetBool();
            CharacterBodyState = (ECharacterBodyState)dataPackage.GetByte();
        }
    }
}