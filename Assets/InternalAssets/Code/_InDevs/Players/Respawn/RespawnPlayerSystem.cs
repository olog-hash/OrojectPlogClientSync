using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Respawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnPlayerSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        
        private EntityLifeProcessor _entityLifeProcessor;
        
        public override void OnAwake()
        {
            _entityLifeProcessor = new EntityLifeProcessor();
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                SpawnPlayer(spawnEvent);
            }
        }

        public void SpawnPlayer(RespawnPlayerEvent respawnEvent)
        {
            if (respawnEvent.PlayerProvider is null || respawnEvent.PlayerProvider.Entity is null) return;
            var playerEntity = respawnEvent.PlayerProvider.Entity;
            
            playerEntity.AddComponentData(new SetPositionRotation()
            {
                Position = respawnEvent.Position,
                Rotation = respawnEvent.Rotation,
            });
            

            if (_entityLifeProcessor.IsDead(playerEntity))
            {
                _entityLifeProcessor.ReviveEntity(playerEntity);
            }
        }
    }
}