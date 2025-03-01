using ProjectOlog.Code.Mechanics.Replenish.Armor;
using ProjectOlog.Code.Mechanics.Replenish.Health;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish.Components;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Infrastructure.SubComponents.Unpackers.Impact
{
    public class ReplenishEventsUnpacker : ImpactEventsUnpacker
    {
        /// <summary>
        /// Распаковка пакета, содержащего данные о пополнении здоровья.
        /// </summary>
        public void UnpackReplenishHealthEventPacket(ReplenishHealthEventPacket packet)
        {
            // Обрабатываем составные данные Impact
            ProcessImpactEventData(packet.ImpactEventData);
            
            foreach (var replenishHealthData in packet.ReplenishHealthDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(replenishHealthData.EventID);
                ProcessReplenishHealth(entity, replenishHealthData);
            }

            // Очищаем контейнер
            ClearEventContainer();
        }

        /// <summary>
        /// Распаковка пакета, содержащего данные о пополнении брони.
        /// </summary>
        public void UnpackReplenishArmorEventPacket(ReplenishArmorEventPacket packet)
        {
            // Обрабатываем составные данные Impact
            ProcessImpactEventData(packet.ImpactEventData);
            
            foreach (var replenishArmorData in packet.ReplenishArmorDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(replenishArmorData.EventID);
                ProcessReplenishArmor(entity, replenishArmorData);
            }

            // Очищаем контейнер
            ClearEventContainer();
        }

        private void ProcessReplenishHealth(Entity entity, ReplenishHealthData replenishHealthData)
        {
            entity.AddComponentData(new ReplenishHealthEvent
            {
                ReplenishCount = replenishHealthData.ReplenishHealthCount
            });
        }
        
        private void ProcessReplenishArmor(Entity entity, ReplenishArmorData replenishArmorData)
        {
            entity.AddComponentData(new ReplenishArmorEvent
            {
                ReplenishCount = replenishArmorData.ReplenishArmorCount
            });
        }
    }
}