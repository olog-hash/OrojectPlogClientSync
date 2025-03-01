using ProjectOlog.Code.Network.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectOlog.Code.Features.Entities.Destruction.SingleDestroy
{
    /// <summary>
    /// Система для уничтожения отдельных сетевых объектов.
    /// Обрабатывает события DestroyNetworkObjectEvent, удаляя указанные объекты.
    /// </summary>
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
                DestroyObject(destroyEvent.ServerID);
                
                // Удаляем дальнейших код ивента, ибо это конечная система.
                entityEvent.Dispose();
            }
        }

        // Удаляем сетевые обьекты по запросу с сервера.
        private void DestroyObject(ushort serverID)
        {
            if (_entitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider))
            {
                _entitiesContainer.RemoveNetworkEntity(serverID);
                entityProvider.AddComponent<RemoveEntityOnDestroy>();
                Object.Destroy(entityProvider.gameObject);
            }
        }
    }
}