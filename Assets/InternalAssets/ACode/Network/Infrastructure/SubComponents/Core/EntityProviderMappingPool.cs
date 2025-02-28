using System.Collections.Generic;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core
{
    public class EntityProviderMappingPool
    {
        public Dictionary<ushort, EntityProvider> EventIDToEntityProvider;
        
        public EntityProviderMappingPool()
        {
            EventIDToEntityProvider = new Dictionary<ushort, EntityProvider>();
        }

        public void Add(ushort eventId, EntityProvider entity)
        {
            EventIDToEntityProvider[eventId] = entity;
        }

        public EntityProvider Get(ushort eventId)
        {
            return EventIDToEntityProvider[eventId];
        }

        public void Clear()
        {
            EventIDToEntityProvider.Clear();
        }
    }
}