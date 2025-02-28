using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core.Components;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.SubComponents.Unpackers.Impact
{
    public class ImpactEventsUnpacker : BasedNetworkUnpacker
    {
        /// <summary>
        /// Унифицированная обработка Impact-данных: агрессоры, жертвы и прочее.
        /// </summary>
        protected void ProcessImpactEventData(ImpactEventData impactData)
        {
            // Обработка данных об агрессоре-entity
            foreach (var aggressorData in impactData.EntityAggressorDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(aggressorData.EventID);
                ProcessEntityAggressor(entity, aggressorData);
            }

            // Обработка данных об агрессоре-среды
            foreach (var envAggressorData in impactData.EnvironmentAggressionDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(envAggressorData.EventID);
                ProcessEnvironmentAggressor(entity, envAggressorData);
            }

            // Обработка данных о жертве
            foreach (var victimData in impactData.EntityVictimDatas)
            {
                Entity entity = GetOrCreateTickEventEntity(victimData.EventID);
                ProcessEntityVictim(entity, victimData);
            }
        }
        
        private void ProcessEntityAggressor(Entity entity, EntityAggressorData entityAggressorData)
        {
            entity.AddComponentData(new EntityAggressorEvent
            {
                AggressorEntity = GetNetworkEntityByServerID(entityAggressorData.ServerID)
            });
        }

        private void ProcessEnvironmentAggressor(Entity entity, EnvironmentAggressionData environmentAggressionData)
        {
            entity.AddComponentData(new EnvironmentAggressorEvent
            {
                EnvironmentType = environmentAggressionData.EnvironmentType
            });
        }
        
        private void ProcessEntityVictim(Entity entity, EntityVictimData entityVictimData)
        {
            entity.AddComponentData(new EntityVictimEvent
            {
                VictimEntity = GetNetworkEntityByServerID(entityVictimData.ServerID)
            });
        }
    }
}