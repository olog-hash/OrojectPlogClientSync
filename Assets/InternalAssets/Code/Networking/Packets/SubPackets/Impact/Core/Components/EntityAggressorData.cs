using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Impact
{
    public class EntityAggressorData : BaseSubPacketData
    {
        public int ServerID;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ServerID);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            ServerID = dataPackage.GetInt();
        }
    }
}