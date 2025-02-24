using ProjectOlog.Code.Mechanics.Mortality.Damage;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Mortality;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Mortality.Components;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.SubComponents.Unpackers.Impact
{
    public class MortalityEventsUnpacker : ImpactEventsUnpacker
    {
        /// <summary>
        /// Распаковка пакета, содержащего данные о нанесённом уроне.
        /// </summary>
        public void UnpackDamageEventPacket(DamageEventPacket packet)
        {
            // Обрабатываем составные данные Impact
            ProcessImpactEventData(packet.ImpactEventData);
            
            foreach (var damageData in packet.DamageDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(damageData.EventID);
                ProcessDamage(entity, damageData);
            }

            // Очищаем контейнер
            ClearEventContainer();
        }

        /// <summary>
        /// Распаковка пакета, содержащего данные о смерти.
        /// </summary>
        public void UnpackDeathEventPacket(DeathEventPacket packet)
        {
            // Обрабатываем составные данные Impact
            ProcessImpactEventData(packet.ImpactEventData);
            
            foreach (var deathData in packet.DeathDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(deathData.EventID);
                ProcessDeath(entity, deathData);
            }

            // Очищаем контейнер
            ClearEventContainer();
        }
        
        private void ProcessDamage(Entity entity, DamageData damageData)
        {
            // Добавляем компонент с данными о нанесённом уроне
            entity.AddComponentData(new PostDamageEvent
            {
                ActualDamageCount = damageData.DamageCount
            });
        }
        
        private void ProcessDeath(Entity entity, DeathData deathData)
        {
            entity.AddComponent<DeathEvent>();
        }
    }
}