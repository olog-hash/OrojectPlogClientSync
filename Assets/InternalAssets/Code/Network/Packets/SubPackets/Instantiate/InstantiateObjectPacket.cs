using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate
{
    public class InstantiateObjectPacket : INetPackageSerializable
    {
        public int Length => NetworkObjectDatas.Length;
        
        // Базовая сетевая информация
        public NetworkIdentityData[] NetworkIdentityDatas;
        public NetworkObjectData[] NetworkObjectDatas;
        public NetworkTransformData[] NetworkTransformDatas;

        // Компоненты
        public TransferObjectData[] TransferObjectDatas;
        public InteractionObjectData[] InteractionObjectDatas;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                NetworkIdentityDatas,
                NetworkObjectDatas,
                NetworkTransformDatas,
                TransferObjectDatas,
                InteractionObjectDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            NetworkIdentityDatas = dataPackage.GetHeadlessCustomArray<NetworkIdentityData>();
            NetworkObjectDatas = dataPackage.GetHeadlessCustomArray<NetworkObjectData>();
            NetworkTransformDatas = dataPackage.GetHeadlessCustomArray<NetworkTransformData>();
            
            TransferObjectDatas = dataPackage.GetHeadlessCustomArray<TransferObjectData>();
            InteractionObjectDatas = dataPackage.GetHeadlessCustomArray<InteractionObjectData>();
        }
    }
}