using ProjectOlog.Code.Network.Gameplay.Core.Enums;

namespace ProjectOlog.Code.Infrastructure.ResourceManagement
{
    public static class NetworkObjectRegistration
    {
        public static void RegisterNetworkObjects()
        {
            NetworkObjectRegistry.Reset();
            
            // Пример регистрации сетевых объектов
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.None, "ERROR");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.ERROR_Look, "ERROR_Look");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Grass_Block, "Grass_Block");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.TransferCube, "TransferCube");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.ActiveDoor, "DoorObject");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.MusicPlayer, "MusicObject");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.LampBlock, "Lamp");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Remover, "Remover");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Picture, "Picture");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.KitPack, "KitPack");
            
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Ammo_KitPack, "AmmoKit");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Ammo_Small_KitPack, "AmmoKitSmall");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Armor_KitPack, "ArmorKit");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Armor_Small_KitPack, "ArmorKitSmall");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Health_KitPack, "HealthKit");
            NetworkObjectRegistry.RegisterNetworkObject(ENetworkObjectType.Health_Small_KitPack, "HealthKitSmall");
        }
    }
}