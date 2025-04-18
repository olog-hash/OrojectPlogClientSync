﻿using ProjectOlog.Code._InDevs.Players.Init;
using ProjectOlog.Code._InDevs.Players.PostInit;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code._InDevs.Players.SyncCommon;
using ProjectOlog.Code._InDevs.Players.SyncObjectActions;
using ProjectOlog.Code._InDevs.Players.Visual.CrossPanel;
using ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer;
using ProjectOlog.Code.Networking.Game.Snapshot.LocalSync;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Gameplay.ECS.Systems.Features
{
    public sealed class PlayerSystemsFeature : FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<InitLocalPlayerSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InitRemotePlayerSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<PostInitPlayerDeathSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<RespawnPlayerSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<LocalSyncSenderSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<SyncPlayerInteractionObjectSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<RespawnLocalPlayerReceivedSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<ShieldPlayerProtectSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<PlayerCrosshairSystem>(_systemsGroup);
        }
    }
}