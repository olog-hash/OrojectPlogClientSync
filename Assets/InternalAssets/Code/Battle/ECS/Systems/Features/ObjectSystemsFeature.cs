using ProjectOlog.Code._InDevs.KitPacksInteract.AudioSound;
using ProjectOlog.Code.Features.Entities.Destruction.ListDestroy;
using ProjectOlog.Code.Features.Entities.Destruction.SingleDestroy;
using ProjectOlog.Code.Features.Objects.Instantiate;
using ProjectOlog.Code.Features.Objects.Interactables.Core.Systems;
using ProjectOlog.Code.Features.Players.SyncObjectActions;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class ObjectSystemsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<InstantiateObjectSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<InstantiateObjectExpandedSystem>(_systemsGroup); 
            
            _systemsFactory.CreateSystem<SyncPlayerSpawnObjectSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InteractiveLogicUpdateSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<RemoteStateTransitionBroadcastSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<RemoteDataBroadcastSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<KitPackAudioSystem>(_systemsGroup);
            
            // Хотя она используется как для игроков так и для обьектов.
            _systemsFactory.CreateSystem<DestroyObjectsListSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<DestroyNetworkObjectSystem>(_systemsGroup); 
        }
    }
}