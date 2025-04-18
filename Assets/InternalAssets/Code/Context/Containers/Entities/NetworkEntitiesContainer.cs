using System.Linq;
using ProjectOlog.Code.Battle.ECS.Rules;
using ProjectOlog.Code.DataStorage.Core;
using ProjectOlog.Code.Network.Profiles.Entities.Containers;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Network.Profiles.Entities
{
    public sealed class NetworkEntitiesContainer : ISceneContainer
    {
        public EcsRules EcsRules;

        public PlayerBaseEntityContainer PlayerBaseEntities { get; } = new PlayerBaseEntityContainer();
        public ObjectBaseEntityContainer ObjectBaseEntities { get; } = new ObjectBaseEntityContainer();
        
        // Статистика
        public int TotalEntitiesCount => PlayerBaseEntities.Count + ObjectBaseEntities.Count;
        
        public void Reset()
        {
            ObjectBaseEntities.Clear();
            PlayerBaseEntities.Clear();
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
            return ObjectBaseEntities.GetNetworkEntity(id) ?? 
                   PlayerBaseEntities.GetNetworkEntity(id);
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
            bool entityRemoved = PlayerBaseEntities.RemoveNetworkEntity(id) || 
                                 ObjectBaseEntities.RemoveNetworkEntity(id);
                                 
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
            return PlayerBaseEntities.ContainsEntityWithId(id) || 
                   ObjectBaseEntities.ContainsEntityWithId(id);
        }
        
        /// <summary>
        /// Получает массив всех активных ServerID (и игроков, и объектов)
        /// </summary>
        public ushort[] GetAllNetworkIds()
        {
            return PlayerBaseEntities.GetAllEntityIds()
                .Concat(ObjectBaseEntities.GetAllEntityIds())
                .ToArray();
        }
        
        /// <summary>
        /// Получает отчет об использовании ID и количестве сущностей
        /// </summary>
        public string GetContainerStatus()
        {
            return $"Сетевые сущности: {TotalEntitiesCount} (Игроки: {PlayerBaseEntities.Count}, Объекты: {ObjectBaseEntities.Count})\n";
        }
    }
}