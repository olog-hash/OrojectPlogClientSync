using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components
{
    public class ShieldProtectData : BaseSubPacketData
    {
        public float ShieldTime;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ShieldTime);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);

            ShieldTime = dataPackage.GetFloat();
        }
    }
}