using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Mortality.PostDamage;
using ProjectOlog.Code.Networking.Packets.Mortality;
using ProjectOlog.Code.Networking.Packets.Mortality.Components;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.CompoundEvents.Unpackers
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