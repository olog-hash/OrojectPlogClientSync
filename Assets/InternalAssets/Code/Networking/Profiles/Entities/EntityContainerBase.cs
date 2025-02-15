using System.Collections;
using System.Collections.Generic;
using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Networking.Profiles.Entities
{
    public abstract class EntityContainerBase : IEnumerable<EntityProvider>
    {
        protected List<EntityProvider> Entities { get; } = new List<EntityProvider>();

        public void Clear() => Entities.Clear();

        public void AddEntity(EntityProvider entityProvider)
        {
            if (IsAvaliableToAdd(entityProvider))
            {
                Entities.Add(entityProvider);
            }
        }

        public EntityProvider GetNetworkEntity(int id)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                ref var networkIdentity = ref Entities[i].Entity.GetComponent<NetworkIdentity>();

                if (networkIdentity.ServerID == id)
                {
                    return Entities[i];
                }
            }

            return null;
        }

        public bool RemoveNetworkEntity(int id)
        {
            var entity = GetNetworkEntity(id);
            return entity != null && Entities.Remove(entity);
        }

        public bool TryGetNetworkEntity(int id, out EntityProvider entityProvider)
        {
            entityProvider = GetNetworkEntity(id);
            return entityProvider != null;
        }

        public virtual bool IsAvaliableToAdd(EntityProvider entityProvider)
        {
            return entityProvider.Has<NetworkIdentity>();
        }

        public IEnumerator<EntityProvider> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        // Индексатор для доступа к элементам списка Entities
        public EntityProvider this[int index] => Entities[index];

        // Свойство для получения количества элементов в Entities
        public int Count => Entities.Count;
    }
}