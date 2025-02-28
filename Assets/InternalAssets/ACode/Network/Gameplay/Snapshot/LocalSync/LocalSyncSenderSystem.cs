using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Interpolation;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Logger;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core;
using ProjectOlog.Code.Networking.Packets.SystemSync.Send;
using ProjectOlog.Code.Networking.Profiles.Snapshots.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Networking.Game.Snapshot.LocalSync
{
    /// <summary>
    /// Формирует и отправляет на сервер клиентский апдейт, включающий:
    /// - Состояние локального игрока (позиция, вращение, анимации)
    /// - Техническую информацию (последний известный серверный тик)
    /// - Служебные данные для синхронизации
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LocalSyncSenderSystem : TickrateSystem
    {
        private Filter _localPlayerFilter;

        private SyncGameNetworker _syncGameNetworker;
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public LocalSyncSenderSystem(SyncGameNetworker syncGameNetworker, LocalPlayerMonitoring localPlayerMonitoring)
        {
            _syncGameNetworker = syncGameNetworker;
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {
            _localPlayerFilter = World.Filter.With<LocalPlayerMarker>().With<CharacterBodyLogger>().With<Translation>().Without<DeadMarker>().Build();
        }

        // Просто тупо берет локального игрока и передает его данные
        public override void OnUpdate(float deltaTime)
        {
            var localPlayerStateData = GetLocalPlayerSyncData();
            var lastPlayerStateVersion = GetLocalPlayerStateVersion();
            
            var clientSyncPacket = new PlayerStateClientPacket()
            {
                LastKnownServerTick = NetworkTime.LastServerTick,
                LastStateVersion = lastPlayerStateVersion,
                RemoteTime = NetworkTime.localTime,
                
                PlayerTransformStateData = localPlayerStateData,
            };
            
            _syncGameNetworker.SyncPlayerRequest(clientSyncPacket.GetPackage());
        }

        private ushort GetLocalPlayerStateVersion()
        {
            foreach (var localPlayer in _localPlayerFilter)
            {
                ref var translation = ref localPlayer.GetComponent<Translation>();
                ref var networkPlayer = ref localPlayer.GetComponent<NetworkPlayer>();

                return networkPlayer.LastStateVersion;
            }

            return 0;
        }

        private PlayerTransformStateData GetLocalPlayerSyncData()
        {
            if (_localPlayerMonitoring.IsDead())
            {
                //return new ClientSyncPlayerStateData();;
            }

            foreach (var localPlayer in _localPlayerFilter)
            {
                ref var translation = ref localPlayer.GetComponent<Translation>();
                ref var networkPlayer = ref localPlayer.GetComponent<NetworkPlayer>();
                ref var characterInterpolation = ref localPlayer.GetComponent<CharacterInterpolation>();
                ref var characterBodyLogger = ref localPlayer.GetComponent<CharacterBodyLogger>();

                var playerPosition = DeltaConverter.NormalizeVector3(characterInterpolation.CurrentTransform.Position);

                var playerStateData = new PlayerTransformStateData()
                {
                    Position = playerPosition,
                    YawDegrees = translation.rotation.eulerAngles.y,
                    PitchDegrees = characterBodyLogger.ViewPitchDegrees,

                    IsGrounded = characterBodyLogger.IsGrounded,
                    PreviousFallVelocity = characterBodyLogger.PreviousFallVelocity,
                    CharacterBodyState = characterBodyLogger.CharacterBodyState,
                };

                return playerStateData;
            }
            
            return new PlayerTransformStateData();
        }
    }
}