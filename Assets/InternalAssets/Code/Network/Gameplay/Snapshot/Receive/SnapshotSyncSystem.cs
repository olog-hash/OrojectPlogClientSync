using ProjectOlog.Code.Engine.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Objects.Interpolation;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Features.Players.Interpolation;
using ProjectOlog.Code.Network.Profiles.Entities;
using ProjectOlog.Code.Network.Profiles.Snapshots;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Network.Gameplay.Snapshot.Receive
{
    /// <summary>
    /// Система синхронизации состояния сетевых сущностей на основе серверных снапшотов
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SnapshotSyncSystem : TickrateSystem
    {
        private Filter _snapshotEventFilter;

        private NetworkEntitiesContainer _entitiesContainer;
        private NetworkSnapshotContainer _snapshotContainer;

        public SnapshotSyncSystem(NetworkEntitiesContainer entitiesContainer, NetworkSnapshotContainer snapshotContainer)
        {
            _entitiesContainer = entitiesContainer;
            _snapshotContainer = snapshotContainer;
        }

        public override void OnAwake()
        {
            _snapshotEventFilter = World.Filter
                .With<ServerSnapshotEvent>()
                .Build();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            foreach (var eventEntity in _snapshotEventFilter)
            {
                ref var snapshotEvent = ref eventEntity.GetComponent<ServerSnapshotEvent>();
                
                SyncRemotePlayerStates(snapshotEvent.LastServerTick);
                SyncRemoteObjectStates(snapshotEvent.LastServerTick);
                
                eventEntity.Dispose();
            }
        }

        // Синхронизация состояния удаленных игроков
        private void SyncRemotePlayerStates(uint lastServerTick)
        {
            var currentSnapshot = _snapshotContainer.GetSnapshot(lastServerTick);
            if (currentSnapshot == null)
                return;
            
            // Обновляем данные каждого игрока, в соответствии со снапшотом.
            foreach (var playerData in currentSnapshot.PlayersTransform)
            {
                var playerID = playerData.Key;
                var currentTransform = playerData.Value;
                
                // Если такой сущности нет в мире - пропускаем
                if (!_entitiesContainer.PlayerEntities.TryGetPlayerEntity(playerID, out var playerProvider))
                    continue;

                var playerEntity = playerProvider.Entity;
                
                // Если это локальный игрок (мы) - пропускаем.
                if (playerEntity.Has<LocalPlayerMarker>())
                    continue;

                if (playerEntity.TryGetComponent<RemotePlayerInterpolationComponent>(out var mirrorInterpolationComponent))
                {
                    var snap = new RemotePlayerInterpolationSnapshot()
                    {
                        remoteTime = currentTransform.RemoteTime,

                        Position = currentTransform.Position,
                        Rotation = Quaternion.Euler(0, currentTransform.YawDegrees, 0),

                        ViewPitchDegrees = currentTransform.PitchDegrees,
                        CharacterBodyState = (ECharacterBodyState)currentTransform.CharacterBodyState,
                        IsGrounded = currentTransform.IsGrounded,
                    };

                    mirrorInterpolationComponent.RemotePlayerInterpolation.OnMessage(snap);
                }
            }
        }
        
        // Синхронизация удаленных обьектов
        private void SyncRemoteObjectStates(uint lastServerTick)
        {
            var currentSnapshot = _snapshotContainer.GetSnapshot(lastServerTick);
            if (currentSnapshot == null)
                return;
            
            // Обновляем данные каждого игрока, в соответствии со снапшотом.
            foreach (var objectData in currentSnapshot.ObjectsTransform)
            {
                var objectID = objectData.Key;
                var currentTransform = objectData.Value;
                
                // Если такой сущности нет в мире - пропускаем
                if (!_entitiesContainer.TryGetNetworkEntity(objectID, out var objectProvider))
                    continue;

                var objectEntity = objectProvider.Entity;


                if (objectEntity.Has<RemoteObjectInterpolationComponent>())
                {
                    ref var translation = ref objectEntity.GetComponent<Translation>();
                    ref var remoteObjectInterpolation = ref objectEntity.GetComponent<RemoteObjectInterpolationComponent>();

                    var position = translation.position;
                    var rotation = translation.rotation;
                    var scale = translation.scale;

                    if (currentTransform.Position.HasValue)
                    {
                        position = currentTransform.Position.Value;
                    }

                    if (currentTransform.Rotation.HasValue)
                    {
                        rotation = Quaternion.Euler(currentTransform.Rotation.Value);
                    }

                    if (currentTransform.Scale.HasValue)
                    {
                        scale = currentTransform.Scale.Value;
                    }

                    

                    var remoteObjectSnapshot = new RemoteObjectInterpolationSnapshot(currentSnapshot.ServerTime, 0f, position, rotation, scale);
                    remoteObjectInterpolation.RemoteObjectInterpolation.OnMessage(remoteObjectSnapshot);

                }
            }
        }
    }
}