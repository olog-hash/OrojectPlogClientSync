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
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyObjectsListSystem : TickrateSystem
    {
        private Filter _clientMassDestroyFilter;
        private NetworkEntitiesContainer _entitiesContainer;

        public DestroyObjectsListSystem(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }

        public override void OnAwake()
        {
            _clientMassDestroyFilter = World.Filter.With<DestroyObjectsListEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _clientMassDestroyFilter)
            {
                ref var massDestroyEvent = ref entity.GetComponent<DestroyObjectsListEvent>();
                ProcessMassDestroy(massDestroyEvent.DestructibleObjectIds);
            }
        }

        private void ProcessMassDestroy(int[] destructibleObjectIds)
        {
            var objectsToDestroy = _entitiesContainer.ObjectEntities
                .Where(obj => destructibleObjectIds.Contains(obj.Entity.GetComponent<NetworkIdentity>().ServerID))
                .ToList();

            foreach (var objectToDestroy in objectsToDestroy)
            {
                DestroyObject(objectToDestroy);
            }
        }

        private void DestroyObject(EntityProvider objectToDestroy)
        {
            ref var networkIdentity = ref objectToDestroy.Entity.GetComponent<NetworkIdentity>();
            _entitiesContainer.RemoveNetworkEntity(networkIdentity.ServerID);
            objectToDestroy.AddComponent<RemoveEntityOnDestroy>();
            Object.Destroy(objectToDestroy.gameObject);
        }
    }
}