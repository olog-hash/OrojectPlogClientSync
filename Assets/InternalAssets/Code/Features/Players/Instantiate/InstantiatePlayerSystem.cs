using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Battle.Context;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components;
using ProjectOlog.Code.Network.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Instantiate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InstantiatePlayerSystem : TickrateSystem
    {
        private Filter _initPlayerFilter;
        
        private NetworkEntitiesContainer _entitiesContainer;
        private BattleContentFactory _battleContentFactory;

        public InstantiatePlayerSystem(NetworkEntitiesContainer entitiesContainer, BattleContentFactory battleContentFactory)
        {
            _entitiesContainer = entitiesContainer;
            _battleContentFactory = battleContentFactory;
        }

        public override void OnAwake()
        {
            _initPlayerFilter = World.Filter.With<InstantiatePlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initPlayerFilter)
            {
                ref var instantiatePlayerEvent = ref entityEvent.GetComponent<InstantiatePlayerEvent>();
                ref var mapping = ref instantiatePlayerEvent.EntityProviderMappingPool;
                var packet = instantiatePlayerEvent.InstantiatePlayerPacket;

                ProcessNetworkPlayers(packet.NetworkPlayerDatas, ref mapping);
                ProcessNetworkIdentities(packet.NetworkIdentityDatas, ref mapping);
                ProcessNetworkTransforms(packet.NetworkTransformDatas, ref mapping);
            }
        }
        
        private void ProcessNetworkPlayers(NetworkPlayerData[] networkPlayerDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var networkPlayer in networkPlayerDatas)
            {
                var provider = CreatePlayer(networkPlayer.UserID);
                provider.Entity.SetComponent(new NetworkPlayer { UserID = networkPlayer.UserID, LastStateVersion = networkPlayer.LastStateVersion });
                
                // Добавляем в словарь ссылку на сущность для других систем.
                mapping.EventIDToEntityProvider.Add(networkPlayer.EventID, provider);
                
                // Создаем ивент "пост-init"
                World.CreateTickEvent().AddComponentData(new PostInstantiatePlayerEvent()
                    { PlayerEntity = provider.Entity });
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
                    _entitiesContainer.PlayerEntities.AddEntity(provider);
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

        private EntityProvider CreatePlayer(int userID)
        {
            EntityProvider playerProvider;
            
            if (userID == LocalData.LocalID)
            {
                playerProvider = CreateLocalPlayer();
            }
            else
            {
                playerProvider = CreateRemotePlayer();
            }
            
            return playerProvider;
        }

        private EntityProvider CreateRemotePlayer()
        {
            var entityProvider = Object.Instantiate(_battleContentFactory.ThirdPersonCharacter).GetComponent<EntityProvider>();

            return entityProvider;
        }

        private EntityProvider CreateLocalPlayer()
        {
            var entityProvider = Object.Instantiate(_battleContentFactory.FirstPersonCharacter).GetComponent<EntityProvider>();
            entityProvider.Entity.AddComponent<LocalPlayerMarker>();

            return entityProvider;
        }
    }
}