namespace ProjectOlog.Code.Core.Enums
{
    public enum ClientPacketType : short
    {
        None,
        Serialized,

        RoomRegisterRequest,
        ServerInitializeRequest,
        
        SpawnRequest,
        SyncPlayerDataRequest,
        
        SpawnObjectRequest,
        SyncInteractionObjectRequest,
        
        SendMessage,
    }

    public enum ServerPacketType : short
    {
        None,
        Serialized,

        RoomRegisterReceived,
        ServerInitialized,

        UserConnected,
        UserDisconnected,
        
        InitPlayer,
        SpawnPlayer,
        
        InitObject,
        SyncInteractionObjectRequest,
        
        DestroyNetworkObject,
        
        SyncGameSnapshot,
        SyncMetaNetworkDate,
        InitIncomingMessage,
        
        SyncHealthArmor,
        DamageEntity,
        KillEntity,
        
    }
}