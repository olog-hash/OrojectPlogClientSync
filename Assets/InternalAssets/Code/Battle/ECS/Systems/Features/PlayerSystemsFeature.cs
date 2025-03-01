using ProjectOlog.Code.Features.Entities.ShieldProtect;
using ProjectOlog.Code.Features.Players.Instantiate;
using ProjectOlog.Code.Features.Players.Respawn;
using ProjectOlog.Code.Features.Players.SyncObjectActions;
using ProjectOlog.Code.Features.Players.Visual.CrossPanel;
using ProjectOlog.Code.Network.Gameplay.Snapshot.LocalSync;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class PlayerSystemsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<InstantiatePlayerSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InstantiatePlayerMortalitySystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InstantiatePlayerPresentationSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<RespawnPlayerSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<LocalSyncSenderSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<SyncPlayerInteractionObjectSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<ShieldPlayerProtectSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<PlayerCrosshairSystem>(_systemsGroup);
        }
    }
}