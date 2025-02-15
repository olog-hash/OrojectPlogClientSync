using ProjectOlog.Code.Entities.Objects.Core;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Gameplay.Battle;
using ProjectOlog.Code.Infrastructure.ResourceManagement;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Initialization
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitObjectSystem : TickrateSystem
    {
        private Filter _initObjectEventsFilter;

        private NetworkEntitiesContainer _entitiesContainer;
        private BattleContentFactory _battleContentFactory;

        public InitObjectSystem(NetworkEntitiesContainer entitiesContainer, BattleContentFactory battleContentFactory)
        {
            _entitiesContainer = entitiesContainer;
            _battleContentFactory = battleContentFactory;
        }

        public override void OnAwake()
        {
            _initObjectEventsFilter = World.Filter.With<InitObjectEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initObjectEventsFilter)
            {
                ref var initObjectEvent = ref entityEvent.GetComponent<InitObjectEvent>();
                InitObject(initObjectEvent);
                entityEvent.Dispose();
            }
        }

        private void InitObject(InitObjectEvent initObjectEvent)
        {
            var entityProvider = SpawnObject(initObjectEvent);
            
            if (entityProvider == null)
            {
                Debug.LogError($"Failed to spawn object of type {initObjectEvent.ObjectType}");
                return;
            }

            ConfigureEntity(entityProvider, initObjectEvent);
            
            NetworkObjectDecoder.DecodeObjectData(entityProvider, initObjectEvent.ObjectData);
            _entitiesContainer.ObjectEntities.AddEntity(entityProvider);
        }

        private EntityProvider SpawnObject(InitObjectEvent initObjectEvent)
        {
            var prefab = NetworkObjectRegistry.GetNetworkObjectPrefab(initObjectEvent.ObjectType);
            return Object.Instantiate(prefab, initObjectEvent.Position, initObjectEvent.Rotation)
                .GetComponent<EntityProvider>();
        }

        private void ConfigureEntity(EntityProvider entityProvider, InitObjectEvent initObjectEvent)
        {
            var entity = entityProvider.Entity;

            SetNetworkComponents(entity, initObjectEvent);
            AddRequiredComponents(entityProvider);
            ConfigureInterpolation(entity);
        }

        private void AddRequiredComponents(EntityProvider entityProvider)
        {
            entityProvider.AddComponent<TranslationProvider>();
            entityProvider.AddComponent<InterpolationProvider>();
        }

        private void ConfigureInterpolation(Entity entity)
        {
            ref var interpolation = ref entity.GetComponent<Interpolation>();
            interpolation.InterpolateTranslation = true;
            interpolation.InterpolateRotation = true;
            interpolation.SkipNextInterpolation();
        }

        private void SetNetworkComponents(Entity entity, InitObjectEvent initObjectEvent)
        {
            entity.SetComponent(new NetworkIdentity { ServerID = initObjectEvent.ServerID });
            entity.SetComponent(new NetworkObject { ObjectType = initObjectEvent.ObjectType });
        }
    }
}