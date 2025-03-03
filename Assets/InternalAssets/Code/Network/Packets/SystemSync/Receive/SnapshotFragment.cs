using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SystemSync.Send
{
    // Класс, представляющий фрагмент снапшота
    public class SnapshotFragment : INetPackageSerializable
    {
        public uint SnapshotId;         // ID текущего снапшота
        public uint SnapshotTimestamp;  // Временная метка создания снапшота (мс)
        public byte FragmentIndex;      // Номер фрагмента
        public byte TotalFragments;     // Общее количество фрагментов
        public byte[] FragmentData;     // Данные фрагмента

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(SnapshotId, SnapshotTimestamp, FragmentIndex, TotalFragments, FragmentData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            SnapshotId = dataPackage.GetUInt();
            SnapshotTimestamp = dataPackage.GetUInt();
            FragmentIndex = dataPackage.GetByte();
            TotalFragments = dataPackage.GetByte();
            FragmentData = dataPackage.GetByteArray();
        }
    }
}