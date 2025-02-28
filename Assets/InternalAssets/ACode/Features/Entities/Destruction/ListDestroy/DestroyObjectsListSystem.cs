using System.Linq;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;

namespace ProjectOlog.Code.Entities.Objects.Destruction
{
    /// <summary>
    /// Система массового уничтожения игровых объектов.
    /// Обрабатывает события DestroyObjectsListEvent, удаляя указанные объекты.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyObjectsListSystem : TickrateSystem
    {
        private Filter _destroyListFilter;
        private NetworkEntitiesContainer _entitiesContainer;

        public DestroyObjectsListSystem(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }

        public override void OnAwake()
        {
            _destroyListFilter = World.Filter.With<DestroyObjectsListEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _destroyListFilter)
            {
                ref var massDestroyEvent = ref entityEvent.GetComponent<DestroyObjectsListEvent>();
                ProcessMassDestroy(massDestroyEvent.DestructibleObjectIds);
                
                // Удаляем дальнейших код ивента, ибо это конечная система.
                entityEvent.Dispose();
            }
        }

        private void ProcessMassDestroy(ushort[] destructibleObjectIds)
        {
            // Уничтожаем объекты
            for (int i = 0; i < destructibleObjectIds.Length; i++)
            {
                DestroyObject(destructibleObjectIds[i]);
            }
        }

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