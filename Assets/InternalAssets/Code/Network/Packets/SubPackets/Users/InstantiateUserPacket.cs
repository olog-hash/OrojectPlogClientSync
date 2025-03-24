using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Users.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Users
{
    public class InstantiateUserPacket : INetPackageSerializable
    {
        public UserDataPacket[] UserDataPackets;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(UserDataPackets);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            UserDataPackets = dataPackage.GetHeadlessCustomArray<UserDataPacket>();
        }
    }
}