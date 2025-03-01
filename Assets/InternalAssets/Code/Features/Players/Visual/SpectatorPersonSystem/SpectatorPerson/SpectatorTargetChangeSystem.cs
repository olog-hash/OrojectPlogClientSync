using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Engine.Cameras.Services;
using ProjectOlog.Code.Engine.Cameras.Types;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpectatorTargetChangeSystem : TickrateSystem
    {
        private Filter _switchPersonRequestFilter;
        private readonly LocalPlayerMonitoring _localPlayerMonitoring;
        private readonly MainBattleCamera _mainBattleCamera;
        private readonly SpectatorCameraService _spectatorCameraService;

        public SpectatorTargetChangeSystem(LocalPlayerMonitoring localPlayerMonitoring,
            MainBattleCamera mainBattleCamera)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
            _mainBattleCamera = mainBattleCamera;

            _spectatorCameraService = new SpectatorCameraService(_mainBattleCamera);
        }

        public override void OnAwake()
        {
            _switchPersonRequestFilter = World.Filter.With<SpectatorTargetChangeRequestEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _switchPersonRequestFilter)
            {
                ref var switchSpectatorRequest = ref entityEvent.GetComponent<SpectatorTargetChangeRequestEvent>();
                TrySwitchToPlayer(switchSpectatorRequest.SpectatorTarget);
            }
        }

        private bool ValidateTarget(Entity targetPlayer)
        {
            if (targetPlayer is null)
            {
                //Debug.Log("[SpectatorSystem] TargetPlayer is null");
                return false;
            }

            if (!_localPlayerMonitoring.IsDead())
            {
                Debug.Log("[SpectatorSystem] The player is alive!");
                return false;
            }

            if (!targetPlayer.TryGetComponent<ThirdPersonOrbitTarget>(out _))
            {
                Debug.Log("[SpectatorSystem] Target has no orbit component");
                return false;
            }

            return true;
        }

        private void SetupCamera(ThirdPersonOrbitTarget orbitTarget)
        {
            _spectatorCameraService.SetTarget(orbitTarget.ThirdPersonOrbit);
            _spectatorCameraService.SetZoomAbility(true, true);
            _spectatorCameraService.SetCameraActive(orbitTarget.ThirdPersonOrbit, true);
        }

        private void TrySwitchToPlayer(Entity targetPlayer)
        {
            if (!ValidateTarget(targetPlayer))
            {
                return;
            }

            SetupCamera(targetPlayer.GetComponent<ThirdPersonOrbitTarget>());
        }
    }
}