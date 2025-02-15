using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Core;

namespace ProjectOlog.Code.Networking.Packets.Repercussion
{
    public class RepercussionPacket : INetPackageSerializable
    {
        public EPressureType PressureType;

        public NetDataPackage PressurePacket;
        public NetDataPackage DetailPacket;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage((ushort)PressureType, PressurePacket, DetailPacket);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            PressureType = (EPressureType)dataPackage.GetUShort();

            PressurePacket = dataPackage.GetPackage();
            DetailPacket = dataPackage.GetPackage();
        }
    }
}