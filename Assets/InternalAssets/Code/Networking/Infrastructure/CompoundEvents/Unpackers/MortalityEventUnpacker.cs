using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Mortality.PostDamage;
using ProjectOlog.Code.Networking.Packets.Mortality;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.CompoundEvents.Unpackers
{
    public class MortalityEventUnpacker : NetworkEventUnpacker
    {
        /// <summary>
        /// Распаковка пакета, содержащего данные о нанесённом уроне.
        /// </summary>
        public void UnpackDamageEventPacket(DamageEventPacket packet)
        {
            // Обрабатываем составные данные Impact
            ProcessImpactEventData(packet.ImpactEventData);

            // Обрабатываем данные о Damage
            foreach (var damageData in packet.DamageDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(damageData.EventID);
                // Добавляем компонент с данными о нанесённом уроне
                entity.AddComponentData(new PostDamageEvent
                {
                    ActualDamageCount = damageData.DamageCount
                });
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

            // Обрабатываем данные о смерти
            foreach (var deathData in packet.DeathDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(deathData.EventID);
                entity.AddComponent<DeathEvent>();
            }

            // Очищаем контейнер
            ClearEventContainer();
        }

        /// <summary>
        /// Унифицированная обработка Impact-данных: агрессоры, жертвы и прочее.
        /// </summary>
        private void ProcessImpactEventData(ImpactEventData impactData)
        {
            // Обработка данных об агрессоре-entity
            foreach (var aggressor in impactData.EntityAggressorDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(aggressor.EventID);
                entity.AddComponentData(new EntityAggressorEvent
                {
                    AggressorEntity = GetNetworkEntityByServerID(aggressor.ServerID)
                });
            }

            // Обработка данных об агрессоре-среды
            foreach (var envAggressor in impactData.EnvironmentAggressionDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(envAggressor.EventID);
                entity.AddComponentData(new EnvironmentAggressorEvent
                {
                    EnvironmentType = envAggressor.EnvironmentType
                });
            }

            // Обработка данных о жертве
            foreach (var victim in impactData.EntityVictimDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(victim.EventID);
                entity.AddComponentData(new EntityVictimEvent
                {
                    VictimEntity = GetNetworkEntityByServerID(victim.ServerID)
                });
            }
        }
    }
}