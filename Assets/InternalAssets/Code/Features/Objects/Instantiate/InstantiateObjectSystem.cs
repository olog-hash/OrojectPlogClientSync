using ProjectOlog.Code.Battle.Context;
using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Objects.Interpolation;
using ProjectOlog.Code.Infrastructure.ResourceManagement;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components;
using ProjectOlog.Code.Network.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectOlog.Code.Features.Objects.Instantiate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InstantiateObjectSystem : TickrateSystem
    {
        private Filter _initObjectFilter;
        
        private NetworkEntitiesContainer _entitiesContainer;
        private BattleContentFactory _battleContentFactory;

        public InstantiateObjectSystem(NetworkEntitiesContainer entitiesContainer, BattleContentFactory battleContentFactory)
        {
            _entitiesContainer = entitiesContainer;
            _battleContentFactory = battleContentFactory;
        }

        public override void OnAwake()
        {
            _initObjectFilter = World.Filter.With<InstantiateObjectEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initObjectFilter)
            {
                ref var instantiateObjectEvent = ref entityEvent.GetComponent<InstantiateObjectEvent>();
                ref var mapping = ref instantiateObjectEvent.EntityProviderMappingPool;
                var packet = instantiateObjectEvent.InstantiateObjectPacket;

                ProcessNetworkObject(packet.NetworkObjectDatas, ref mapping);
                ProcessNetworkIdentities(packet.NetworkIdentityDatas, ref mapping);
                ProcessNetworkTransforms(packet.NetworkTransformDatas, ref mapping);
                ProcessBasedComponents(ref mapping);
            }
        }
        
        private void ProcessNetworkObject(NetworkObjectData[] networkObjectDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var networkObject in networkObjectDatas)
            {
                var provider = CreateObject(networkObject.NetworkObjectType);
                provider.Entity.SetComponent(new NetworkObject { ObjectType = networkObject.NetworkObjectType });
                
                // Добавляем в словарь ссылку на сущность для других систем.
                mapping.EventIDToEntityProvider.Add(networkObject.EventID, provider);
                
                // Создаем ивент "пост-init"
                World.CreateTickEvent().AddComponentData(new PostInstantiateObjectEvent { ObjectEntity = provider.Entity });
            }
        }
        
        private void ProcessNetworkIdentities(NetworkIdentityData[] networkIdentityDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var networkIdentity in networkIdentityDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(networkIdentity.EventID, out var provider))
                {
                    provider.Entity.SetComponent(new NetworkIdentity { ServerID = networkIdentity.ServerID });
                    
                    // Добавляем в контейнер
                    _entitiesContainer.ObjectBaseEntities.AddEntity(provider);
                }
            }
        }

        private void ProcessNetworkTransforms(NetworkTransformData[] networkTransformDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var networkTransform in networkTransformDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(networkTransform.EventID, out var provider))
                {
                    provider.transform.position = networkTransform.Position;
                    provider.transform.rotation = Quaternion.Euler(networkTransform.Rotation);
                }
            }
        }

        // Устанавливаем и настраиваем базовые компоненты
        private void ProcessBasedComponents(ref EntityProviderMappingPool mapping)
        {
            foreach (var entityProvider in mapping.EventIDToEntityProvider.Values)
            {
                entityProvider.AddComponent<TranslationProvider>();
            }
        }
        
        private EntityProvider CreateObject(ENetworkObjectType objectType)
        {
            var prefab = NetworkObjectRegistry.GetNetworkObjectPrefab(objectType);
            return Object.Instantiate(prefab).GetComponent<EntityProvider>();
        }
    }
}