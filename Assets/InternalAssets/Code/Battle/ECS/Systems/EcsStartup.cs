﻿using ProjectOlog.Code.Battle.ECS.Systems.Features;
using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Engine.Interpolation;
using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Objects.Interpolation;
using ProjectOlog.Code.Features.Players.Interpolation;
using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using ProjectOlog.Code.Network.Gameplay.Snapshot.Receive;
using ProjectOlog.Code.Network.Gameplay.UserDataGameUpdate;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Battle.ECS.Systems
{
    public class EcsStartup : MonoBehaviour, IFixedUpdate, ITickUpdate, IUpdate, ILateUpdate
    {
        public int Order;

        private EcsSystemsFactory _systemsFactory;
        
        [SerializeField]
        private SystemsGroup _systemsGroup;

        [Inject]
        public void Construct(RuntimeHelper runtimeHelper, EcsSystemsFactory systemsFactory)
        {
            runtimeHelper.RegisterFixedUpdate(this);
            runtimeHelper.RegisterTickUpdate(this);
            runtimeHelper.RegisterUpdate(this);
            runtimeHelper.RegisterLateUpdate(this);
            
            _systemsFactory = systemsFactory;

            InitializedSystems();
            InitializedRules();
            InitializeClearSystems();

            _systemsGroup.SortSystems();
            
            World.Default.AddSystemsGroup(Order, _systemsGroup);
        }

        private void InitializedSystems()
        {
            _systemsGroup = World.Default.CreateSystemsGroup();

            _systemsFactory.CreateSystem<InterpolationFixedUpdateSystem>(_systemsGroup); 
            _systemsFactory.CreateSystem<CharacterInterpolationFixedUpdateSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<TransferSystem>(_systemsGroup);

            _systemsFactory.CreateFeature<FirstPersonPlayerSystemsFeature>(_systemsGroup);

            _systemsFactory.CreateSystem<SnapshotReceiveSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<SnapshotSyncSystem>(_systemsGroup);

            _systemsFactory.CreateSystem<RemoteObjectInterpolationSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<RemotePlayerInterpolationSystem>(_systemsGroup);
            
            _systemsFactory.CreateFeature<PlayerSystemsFeature>(_systemsGroup);
            _systemsFactory.CreateFeature<ObjectSystemsFeature>(_systemsGroup);
            
            _systemsFactory.CreateFeature<DebuggerSystemsFeature>(_systemsGroup);
            _systemsFactory.CreateFeature<BattleHudFeature>(_systemsGroup);

            _systemsFactory.CreateFeature<DamageRepercussionFeature>(_systemsGroup);
            
            _systemsFactory.CreateFeature<PlayerRespawnViewFeature>(_systemsGroup);
            
            _systemsFactory.CreateFeature<SwitchPersonViewFeature>(_systemsGroup);
            
            _systemsFactory.CreateFeature<PlayerAnimationsFeature>(_systemsGroup);

            _systemsFactory.CreateSystem<UserDataInitDestroySystem>(_systemsGroup);
            _systemsFactory.CreateSystem<UserDataRespawnSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<InterpolationVariableUpdateSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<CharacterInterpolationVariableUpdateSystem>(_systemsGroup);
        }

        private void InitializedRules()
        {
            _systemsFactory.CreateFeature<GameRulesFeature>(_systemsGroup);
        }

        private void InitializeClearSystems()
        {
            _systemsFactory.CreateSystem<ClearFixedEventsSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<ClearTickrateEventsSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<ClearUpdateEventsSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<ClearLateUpdateEventsSystem>(_systemsGroup);
        }

        public void OnFixedUpdate(float deltaTime)
        {
            WorldExtensions.GlobalFixedUpdate(deltaTime);
        }

        public void OnTickUpdate(float deltaTime)
        {
            WorldExtensions.GlobalTickrateUpdate(deltaTime);
        }

        public void OnUpdate(float deltaTime)
        {
            WorldExtensions.GlobalUpdate(deltaTime);
        }

        public void OnLateUpdate(float deltaTime)
        {
            WorldExtensions.GlobalLateUpdate(deltaTime);
        }
    }
}