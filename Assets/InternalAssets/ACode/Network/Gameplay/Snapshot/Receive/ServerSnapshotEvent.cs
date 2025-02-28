using ProjectOlog.Code.Networking.Packets.SystemSync.Receive;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Networking.Game.Snapshot.Receive
{
    [global::System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ServerSnapshotEvent : IComponent
    {
        public uint LastServerTick;
        public double LastServerTime;
        public uint LastClientReceivedTick;
        public ESnapshotBroadcastType BroadcastType;

        public SnapshotPlayerData[] PlayersTransform;
        public SnapshotObjectData[] ObjectsTransform;
    }
}