using System.Collections.Generic;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Infrastructure.SubComponents.Core
{
    public class EntityMappingPool
    {
        public Dictionary<ushort, Entity> EventIDToEntity;
        
        public EntityMappingPool()
        {
            EventIDToEntity = new Dictionary<ushort, Entity>();
        }

        public void Add(ushort eventId, Entity entity)
        {
            EventIDToEntity[eventId] = entity;
        }

        public Entity Get(ushort eventId)
        {
            return EventIDToEntity[eventId];
        }

        public void Clear()
        {
            EventIDToEntity.Clear();
        }
    }
}