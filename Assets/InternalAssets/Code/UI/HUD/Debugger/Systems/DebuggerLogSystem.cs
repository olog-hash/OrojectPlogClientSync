using ProjectOlog.Code.Engine.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Engine.Transform;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Network.Client;
using ProjectOlog.Code.Network.Profiles.Entities;
using ProjectOlog.Code.Network.Profiles.Users;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Debugger.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DebuggerLogSystem : UpdateSystem
    {
        private Filter _playerFilter;
        private float _secondTimer = 0;

        private DebuggerViewModel _debuggerViewModel;
        private NetworkUsersContainer _usersContainer;
        private NetworkEntitiesContainer _entitiesContainer;
        private NetworkClient _networkClient;

        public DebuggerLogSystem(DebuggerViewModel debuggerViewModel, NetworkUsersContainer usersContainer, NetworkEntitiesContainer entitiesContainer, NetworkClient networkClient)
        {
            _debuggerViewModel = debuggerViewModel;
            _usersContainer = usersContainer;
            _entitiesContainer = entitiesContainer;
            _networkClient = networkClient;
        }

        public override void OnAwake()
        {
            _playerFilter = World.Filter.With<LocalPlayerMarker>().With<Translation>().With<KinematicCharacterBody>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            var entity = _playerFilter.FirstOrDefault();
            
            if (entity != null)
            {
                var translation = entity.GetComponent<Translation>();
                var kinematicCharacterBody = entity.GetComponent<KinematicCharacterBody>();
                
                _debuggerViewModel.Position = translation.position;
                _debuggerViewModel.Velocity = kinematicCharacterBody.BaseVelocity;
            }

            _debuggerViewModel.CurrentServerTick = NetworkTime.LastServerTick;
            _debuggerViewModel.CurrentLocalTick = NetworkTime.LastLocalTick;
            
            _secondTimer += deltaTime;
            if (_secondTimer >= 1f)
            {
                _secondTimer -= 1f;
                
                _debuggerViewModel.FrameRate = Mathf.RoundToInt(1.0f / Time.deltaTime);
                
                _debuggerViewModel.Ping = NetworkTime.LastPing;
                _debuggerViewModel.BytesInPerSecond = (int)_networkClient.NetStatistics.BytesReceived;
                _debuggerViewModel.PacketsInPerSecond = (int)_networkClient.NetStatistics.PacketsReceived;
                _debuggerViewModel.BytesOutPerSecond = (int)_networkClient.NetStatistics.BytesSent;
                _debuggerViewModel.PacketsOutPerSecond = (int)_networkClient.NetStatistics.PacketsSent;
                _debuggerViewModel.PacketLoss = _networkClient.NetStatistics.PacketLossPercent;

                _debuggerViewModel.UsersCount = _usersContainer.UsersCount;
                _debuggerViewModel.NetworkEntities = _entitiesContainer.ObjectBaseEntities.Count;
                
                _networkClient.NetStatistics.Reset();
            }
        }
    }
}