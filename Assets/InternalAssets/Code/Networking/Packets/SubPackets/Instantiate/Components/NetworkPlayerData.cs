﻿using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
{
    public class NetworkPlayerData : BaseSubPacketData
    {
        public int UserID;
        public ushort LastStateVersion;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, UserID, LastStateVersion);
        }
        
        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            UserID = dataPackage.GetInt();
            LastStateVersion = dataPackage.GetUShort();
        }
    }
}