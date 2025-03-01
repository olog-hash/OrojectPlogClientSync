using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate
{
    public class InstantiatePlayerPacket : INetPackageSerializable
    {
        public int Length => NetworkPlayerDatas.Length;
        
        // Базовая сетевая информация
        public NetworkIdentityData[] NetworkIdentityDatas;
        public NetworkPlayerData[] NetworkPlayerDatas;
        public NetworkTransformData[] NetworkTransformDatas;
        
        // Смертность
        public HealthArmorData[] HealthArmorDatas;
        public DeadMarkerData[] DeadMarkerDatas;
        public ShieldProtectData[] ShieldProtectDatas;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                NetworkIdentityDatas,
                NetworkPlayerDatas,
                NetworkTransformDatas,
                HealthArmorDatas,
                DeadMarkerDatas,
                ShieldProtectDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            NetworkIdentityDatas = dataPackage.GetHeadlessCustomArray<NetworkIdentityData>();
            NetworkPlayerDatas = dataPackage.GetHeadlessCustomArray<NetworkPlayerData>();
            NetworkTransformDatas = dataPackage.GetHeadlessCustomArray<NetworkTransformData>();
            
            HealthArmorDatas = dataPackage.GetHeadlessCustomArray<HealthArmorData>();
            DeadMarkerDatas = dataPackage.GetHeadlessCustomArray<DeadMarkerData>();
            ShieldProtectDatas = dataPackage.GetHeadlessCustomArray<ShieldProtectData>();
        }
    }
}