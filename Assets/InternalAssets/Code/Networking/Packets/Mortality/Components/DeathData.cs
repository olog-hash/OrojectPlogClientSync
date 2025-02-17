using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Components
{
    public class DeathData : BaseEventData, IHeadlessPackageSerializable
    {
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
        }
    }
}