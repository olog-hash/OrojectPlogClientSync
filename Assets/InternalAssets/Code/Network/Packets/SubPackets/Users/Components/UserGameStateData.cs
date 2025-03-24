using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Users.Components
{
    public class UserGameStateData : IHeadlessPackageSerializable
    {
        public bool IsDead;
        public int Deaths;
        public short Ping;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(IsDead, Deaths, Ping);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            IsDead = dataPackage.GetBool();
            Deaths = dataPackage.GetInt();
            Ping = dataPackage.GetShort();
        }
    }
}