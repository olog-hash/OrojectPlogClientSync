using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Impact
{
    public class EntityAggressorData : BaseEventData, IHeadlessPackageSerializable
    {
        public int ServerID;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ServerID);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
            ServerID = dataPackage.GetInt();
        }
    }
}