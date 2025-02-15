namespace ProjectOlog.Code.Networking.Game.Snapshot.Receive
{
    public enum ESnapshotBroadcastType
    {
        None,
        Global, // Полностью все в полном виде
        Changed, // В полном виде, но только те что изменились
        Delta // Только те что измменились, и с дельта-сжатием
    }
}