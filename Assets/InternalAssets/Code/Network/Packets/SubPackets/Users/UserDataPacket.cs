using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Users.Components
{
    /// <summary>
    /// Единичный пакет на одного пользователя.
    /// </summary>
    public class UserDataPacket : IHeadlessPackageSerializable
    {
        public UserIdentityData IdentityData;
        public UserGameStateData GameStateData;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(IdentityData, GameStateData);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            IdentityData = dataPackage.GetHeadlessCustom<UserIdentityData>();
            GameStateData = dataPackage.GetHeadlessCustom<UserGameStateData>();
        }
    }
}