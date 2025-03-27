using ProjectOlog.Code.Features.Entities.ShieldProtect.Events;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Instantiate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InstantiatePlayerMortalitySystem : TickrateSystem
    {
        private Filter _initPlayerFilter;

        public override void OnAwake()
        {
            _initPlayerFilter = World.Filter.With<InstantiatePlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initPlayerFilter)
            {
                ref var instantiatePlayerEvent = ref entityEvent.GetComponent<InstantiatePlayerEvent>();
                ref var mapping = ref instantiatePlayerEvent.EntityProviderMappingPool;
                var packet = instantiatePlayerEvent.InstantiatePlayerPacket;

                ProcessHealthArmors(packet.HealthArmorDatas, ref mapping);
                ProcessShields(packet.ShieldProtectDatas, ref mapping);
                ProcessDeadMarkers(packet.DeadMarkerDatas, ref mapping);
            }
        }

        private void ProcessHealthArmors(HealthArmorData[] healthArmorDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var healthArmor in healthArmorDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(healthArmor.EventID, out var provider))
                {
                    provider.Entity.SetComponent(new HealthArmorComponent()
                    {
                        MaxHealth = healthArmor.MaxHealth,
                        MaxArmor = healthArmor.MaxArmor,

                        Health = healthArmor.Health,
                        Armor = healthArmor.Armor,
                    });
                }
            }
        }

        // Создаем ивенты о добавлении щита на сущность
        private void ProcessShields(ShieldProtectData[] shieldProtectDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var shieldProtect in shieldProtectDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(shieldProtect.EventID, out var provider))
                {
                    var serverID = provider.Entity.GetComponent<NetworkIdentity>().ServerID;

                    World.CreateTickEvent().AddComponentData(new ShieldAddedEvent()
                    {
                        ServerID = serverID,
                        ShieldTime = shieldProtect.ShieldTime
                    });
                }
            }
        }

        // Создаем "фэйковые" ивенты смерти сущности.
        private void ProcessDeadMarkers(DeadMarkerData[] deadMarkerDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var shieldProtect in deadMarkerDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(shieldProtect.EventID, out var provider))
                {
                    var tickEvent = World.CreateTickEvent().AddFakeEventMarker();
                    tickEvent.AddComponentData(new EntityVictimEvent() { VictimEntity = provider.Entity });
                    tickEvent.AddComponent<DeathEvent>();
                }
            }
        }
    }
}