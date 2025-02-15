using ProjectOlog.Code._InDevs.KitPacksInteract;
using ProjectOlog.Code._InDevs.KitPacksInteract.Modules.AudioSound;
using ProjectOlog.Code._InDevs.Players.SyncObjectActions;
using ProjectOlog.Code.Entities.Objects.Destruction;
using ProjectOlog.Code.Entities.Objects.Initialization;
using ProjectOlog.Code.Entities.Objects.Interactables.Core.Systems;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class ObjectSystemsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<InitObjectSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<SyncPlayerSpawnObjectSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InteractiveLogicUpdateSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<RemoteStateTransitionBroadcastSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<RemoteDataBroadcastSystem>(_systemsGroup);

            _systemsFactory.CreateSystem<KitPackInteractSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<KitPackAudioSystem>(_systemsGroup);
            
            // Хотя она используется как для игроков так и для обьектов.
            _systemsFactory.CreateSystem<DestroyObjectsListSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<DestroyNetworkObjectSystem>(_systemsGroup); 
        }
    }
}