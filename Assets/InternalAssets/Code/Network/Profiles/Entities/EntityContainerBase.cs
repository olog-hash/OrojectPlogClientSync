using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Network.Profiles.Entities
{
    public abstract class EntityContainerBase : IEnumerable<EntityProvider>
    {
        // Единственное хранилище - словарь
        protected Dictionary<ushort, EntityProvider> EntitiesById { get; } = new Dictionary<ushort, EntityProvider>();

        public virtual void Clear()
        {
            EntitiesById.Clear();
        }

        public virtual void AddEntity(EntityProvider entityProvider)
        {
            if (!IsAvaliableToAdd(entityProvider))
                return;

            ref var networkIdentity = ref entityProvider.Entity.GetComponent<NetworkIdentity>();
            ushort serverId = networkIdentity.ServerID;

            EntitiesById[serverId] = entityProvider;
        }

        public EntityProvider GetNetworkEntity(ushort id)
        {
            return EntitiesById.TryGetValue(id, out var entity) ? entity : null;
        }

        public virtual bool RemoveNetworkEntity(ushort id)
        {
            return EntitiesById.Remove(id);
        }

        public bool TryGetNetworkEntity(ushort id, out EntityProvider entityProvider)
        {
            return EntitiesById.TryGetValue(id, out entityProvider);
        }

        public virtual bool IsAvaliableToAdd(EntityProvider entityProvider)
        {
            return entityProvider.Has<NetworkIdentity>();
        }

        // Возвращает только значения словаря для итерации
        public IEnumerator<EntityProvider> GetEnumerator() => EntitiesById.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Свойство для получения количества элементов
        public int Count => EntitiesById.Count;

        // Проверка наличия сущности с конкретным ID
        public bool ContainsEntityWithId(ushort id) => EntitiesById.ContainsKey(id);

        // Получение всех ключей (ServerID) как массив
        public ushort[] GetAllEntityIds() => EntitiesById.Keys.ToArray();
    }
}