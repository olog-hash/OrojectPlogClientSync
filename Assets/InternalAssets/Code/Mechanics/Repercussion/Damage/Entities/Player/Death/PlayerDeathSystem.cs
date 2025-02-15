using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Profiles.Users;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using NetworkPlayer = ProjectOlog.Code.Networking.Game.Core.NetworkPlayer;

namespace ProjectOlog.Code.Mechanics.Repercussion.Damage.Entities.Player.Death
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerDeathSystem : TickrateSystem
    {
        private Filter _playerDeathFilter;
        private EntityLifeProcessor _entityLifeProcessor;

        public override void OnAwake()
        {
            _entityLifeProcessor = new EntityLifeProcessor();
            
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<PlayerVictimMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var deathEvent = ref entityEvent.GetComponent<DeathEvent>();
                
                DeathEvent(deathEvent, entityEvent);
            }
        }

        private void DeathEvent(DeathEvent deathEvent, Entity entityEvent)
        {
            if (!_entityLifeProcessor.IsDead(deathEvent.VictimEntity))
            {
                deathEvent.VictimEntity.AddComponent<DeadMarker>();
            }
        }
    }
}