using System.Linq;
using ProjectOlog.Code.Battle.ECS.Rules;
using ProjectOlog.Code.Network.Profiles.Entities.Containers;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Network.Profiles.Entities
{
    public class NetworkEntitiesContainer
    {
        public EcsRules EcsRules;

        public PlayerEntityContainer PlayerEntities { get; }
        public ObjectEntityContainer ObjectEntities { get; }
        
        // Статистика
        public int TotalEntitiesCount => PlayerEntities.Count + ObjectEntities.Count;

        public NetworkEntitiesContainer()
        {
            PlayerEntities = new PlayerEntityContainer();
            ObjectEntities = new ObjectEntityContainer();
            
            ClearContainer();
        }

        public void ClearContainer()
        {
            ObjectEntities.Clear();
            PlayerEntities.Clear();
        }

        public void RegisterEcsRules(EcsRules ecsRules)
        {
            EcsRules = ecsRules;
        }

        /// <summary>
        /// Получает сетевую сущность по ServerID (из любого контейнера)
        /// </summary>
        public EntityProvider GetNetworkEntity(ushort id)
        {
            // Оптимизировано: сначала проверяем объекты, так как их обычно больше
            return ObjectEntities.GetNetworkEntity(id) ?? 
                   PlayerEntities.GetNetworkEntity(id);
        }

        /// <summary>
        /// Пытается получить сетевую сущность по ServerID
        /// </summary>
        public bool TryGetNetworkEntity(ushort id, out EntityProvider entityProvider)
        {
            entityProvider = GetNetworkEntity(id);
            return entityProvider != null;
        }
        
        /// <summary>
        /// Удаляет сетевую сущность и освобождает её ServerID
        /// </summary>
        public bool RemoveNetworkEntity(ushort id)
        {
            // Удаляем из соответствующего контейнера
            bool entityRemoved = PlayerEntities.RemoveNetworkEntity(id) || 
                                 ObjectEntities.RemoveNetworkEntity(id);
                                 
            // Логируем предупреждение, если ID освобожден, но сущность не найдена
            if (!entityRemoved)
            {
                Debug.LogWarning($"[NetworkEntitiesContainer] Cущность не найдена в контейнерах");
            }
            
            return entityRemoved;
        }
        
        /// <summary>
        /// Проверяет наличие сущности с указанным ServerID
        /// </summary>
        public bool ContainsNetworkEntityWithId(ushort id)
        {
            return PlayerEntities.ContainsEntityWithId(id) || 
                   ObjectEntities.ContainsEntityWithId(id);
        }
        
        /// <summary>
        /// Получает массив всех активных ServerID (и игроков, и объектов)
        /// </summary>
        public ushort[] GetAllNetworkIds()
        {
            return PlayerEntities.GetAllEntityIds()
                .Concat(ObjectEntities.GetAllEntityIds())
                .ToArray();
        }
        
        /// <summary>
        /// Получает отчет об использовании ID и количестве сущностей
        /// </summary>
        public string GetContainerStatus()
        {
            return $"Сетевые сущности: {TotalEntitiesCount} (Игроки: {PlayerEntities.Count}, Объекты: {ObjectEntities.Count})\n";
        }
    }
}