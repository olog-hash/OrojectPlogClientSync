﻿using System.Collections.Generic;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Zenject;

namespace ProjectOlog.Code.Networking.Infrastructure.CompoundEvents
{
    public abstract class NetworkEventUnpacker
    {
        // Словарь для хранения распакованных событий по EventID
        protected Dictionary<ushort, Entity> _eventEntities = new Dictionary<ushort, Entity>();
        protected NetworkEntitiesContainer _entitiesContainer;
        
        [Inject]
        public void Construct(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }
        
        /// <summary>
        /// Получает существующую сущность события по EventID или создаёт новую, если её нет.
        /// </summary>
        protected Entity GetOrCreateUpdateEventEntity(ushort eventID)
        {
            if (!_eventEntities.TryGetValue(eventID, out Entity entity))
            {
                entity = World.Default.CreateUpdateEvent();
                _eventEntities.Add(eventID, entity);
            }

            return entity;
        }
        
        /// <summary>
        /// Получает существующую сущность события по EventID или создаёт новую, если её нет.
        /// </summary>
        protected Entity GetOrCreateTickEventEntity(ushort eventID)
        {
            if (!_eventEntities.TryGetValue(eventID, out Entity entity))
            {
                entity = World.Default.CreateTickEvent();
                _eventEntities.Add(eventID, entity);
            }

            return entity;
        }
        
        /// <summary>
        /// Получает существующую сущность события по EventID или создаёт новую, если её нет.
        /// </summary>
        protected Entity GetOrCreateFixedEventEntity(ushort eventID)
        {
            if (!_eventEntities.TryGetValue(eventID, out Entity entity))
            {
                entity = World.Default.CreateFixedUpdateEvent();
                _eventEntities.Add(eventID, entity);
            }

            return entity;
        }
        
        /// <summary>
        /// Получает существующую сущность события по EventID или создаёт новую, если её нет.
        /// </summary>
        protected Entity GetOrCreateLateEventEntity(ushort eventID)
        {
            if (!_eventEntities.TryGetValue(eventID, out Entity entity))
            {
                entity = World.Default.CreateLateUpdateEvent();
                _eventEntities.Add(eventID, entity);
            }

            return entity;
        }
        
        /// <summary>
        /// Заглушка для получения сущности по серверному ID.
        /// Реализация зависит от вашей ECS-системы.
        /// </summary>
        protected Entity GetNetworkEntityByServerID(int serverID)
        {
            if (_entitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider))
            {
                return entityProvider.Entity;
            }

            return null;
        }

        /// <summary>
        /// Очищает контейнер ивентов
        /// </summary>
        protected void ClearEventContainer()
        {
            // После обработки можно очистить контейнер событий
            _eventEntities.Clear();
        }
    }
}