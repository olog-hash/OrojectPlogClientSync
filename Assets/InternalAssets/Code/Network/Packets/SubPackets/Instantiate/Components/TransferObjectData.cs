using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;
using UnityEngine;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components
{
    public class TransferObjectData : BaseSubPacketData
    {
        public Vector3 LinearVelocity;
        public Vector3 AngularVelocity;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, LinearVelocity, AngularVelocity);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);

            LinearVelocity = dataPackage.GetVector3();
            AngularVelocity = dataPackage.GetVector3();
        }
    }
}