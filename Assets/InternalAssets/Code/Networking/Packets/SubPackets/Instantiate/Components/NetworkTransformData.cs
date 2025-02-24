using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
{
    public class NetworkTransformData : BaseSubPacketData
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, Position, Rotation);
        }
        
        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            Position = dataPackage.GetVector3();
            Rotation = dataPackage.GetVector3();
        }
    }
}