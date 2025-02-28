using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
{
    public class HealthArmorData : BaseSubPacketData
    {
        public ushort MaxHealth;
        public ushort MaxArmor;
        
        public ushort Health;
        public ushort Armor;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, MaxHealth, MaxArmor, Health, Armor);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);

            MaxHealth = dataPackage.GetUShort();
            MaxArmor = dataPackage.GetUShort();
            Health = dataPackage.GetUShort();
            Armor = dataPackage.GetUShort();
        }
    }
}