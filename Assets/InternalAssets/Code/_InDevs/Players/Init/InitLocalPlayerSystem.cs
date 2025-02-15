using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.PostInit;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Gameplay.Battle;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Init
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitLocalPlayerSystem : TickrateSystem
    {
        private const int DEFAULT_HEALTH = 150;
        private const int DEFAULT_MAX_ARMOR = 100;
        private const int DEFAULT_ARMOR = 50;
        
        private Filter _initLocalPlayersFilter;
        
        private NetworkEntitiesContainer _entitiesContainer;
        private BattleContentFactory _battleContentFactory;
        
        public InitLocalPlayerSystem(NetworkEntitiesContainer entitiesContainer, BattleContentFactory battleContentFactory)
        {
            _entitiesContainer = entitiesContainer;
            _battleContentFactory = battleContentFactory;
        }
        
        public override void OnAwake()
        {
            _initLocalPlayersFilter = World.Filter.With<InitPlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initLocalPlayersFilter)
            {
                ref var initPlayerEvent = ref entityEvent.GetComponent<InitPlayerEvent>();
                if (!initPlayerEvent.IsLocalPlayer) continue;
                
                InitLocalPlayer(initPlayerEvent);
            }
        }

        public void InitLocalPlayer(InitPlayerEvent initPlayerEvent)
        {
            EntityProvider entityProvider = null;

            entityProvider = Object
                .Instantiate(_battleContentFactory.FirstPersonCharacter, initPlayerEvent.Position,
                    initPlayerEvent.Rotation)
                .GetComponent<EntityProvider>();

            entityProvider.Entity.AddComponent<LocalPlayerMarker>();

            entityProvider.Entity.AddComponentData(new NetworkIdentity()
            {
                ServerID = initPlayerEvent.ServerID,
                IsLocal = true,
            });

            entityProvider.Entity.AddComponentData(new NetworkPlayer()
            {
                UserID = initPlayerEvent.UserID,
            });

            entityProvider.AddComponentData(new HealthArmorComponent()
            {
                MaxHealth = DEFAULT_HEALTH,
                MaxArmor = DEFAULT_ARMOR,

                Health = DEFAULT_HEALTH,
                Armor = DEFAULT_ARMOR,
            });

            entityProvider.Entity.AddComponentData(new SetPositionRotation()
            {
                Position = initPlayerEvent.Position,
                Rotation = initPlayerEvent.Rotation,
            });

            // Добавляем игрока в контейнер
            _entitiesContainer.PlayerEntities.AddEntity(entityProvider);
            
            // Создаем событие на пост ивент
            PostInitEventLunch(entityProvider, initPlayerEvent);
        }

        private void PostInitEventLunch(EntityProvider entityProvider, InitPlayerEvent initPlayerEvent)
        {
            var postInitEvent = new PostInitPlayerEvent()
            {
                PlayerProvider = entityProvider,
                IsDead = initPlayerEvent.IsDead
            };

            World.CreateTickEvent().AddComponentData(postInitEvent);
        }
    }
}