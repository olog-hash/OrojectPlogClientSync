using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Destruction
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyNetworkObjectSystem : UpdateSystem
    {
        private Filter _destroyNetworkObjectsFilter;

        private NetworkEntitiesContainer _entitiesContainer;

        public DestroyNetworkObjectSystem(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }

        public override void OnAwake()
        {
            _destroyNetworkObjectsFilter = World.Filter.With<DestroyNetworkObjectEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _destroyNetworkObjectsFilter)
            {
                ref var destroyEvent = ref entityEvent.GetComponent<DestroyNetworkObjectEvent>();

                DestroyObject(destroyEvent);
                
                entityEvent.Dispose();
            }
        }

        // Удаляем сетевые обьекты по запросу с сервера.
        public void DestroyObject(DestroyNetworkObjectEvent destroyEvent)
        {
            if (!_entitiesContainer.TryGetNetworkEntity(destroyEvent.ServerID, out var entityProvider)) return;
            
            // Удаляем из контейнера
            _entitiesContainer.RemoveNetworkEntity(destroyEvent.ServerID);
            
            // Вешаем все обходимое для удаления
            entityProvider.AddComponent<RemoveEntityOnDestroy>();
            Object.Destroy(entityProvider.gameObject);

            {
                // Возможно можно доработать.
                // Пример - удалить ассоционные обьекты. 
            }
        }
    }
}