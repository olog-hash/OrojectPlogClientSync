using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended;
using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended.CamerasLogic;
using ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Input.Controls;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.CameraSystem.Player.ViewModes.Systems
{
    /// <summary>
    /// Система, которая меняет вид от лица камеры на 1е, 2е или 3е лицо.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SwitchPersonViewSystem : TickrateSystem
    {
        private Filter _switchViewEvents;
        private Filter _localPlayersFilter;

        private MainBattleCamera _mainBattleCamera;
        private FirstPersonCameraService _firstPersonCameraService;
        private SecondPersonCameraService _secondPersonCameraService;
        private ThirdPersonCameraService _thirdPersonCameraService;

        public SwitchPersonViewSystem(MainBattleCamera mainBattleCamera)
        {
            _mainBattleCamera = mainBattleCamera;

            _firstPersonCameraService = new FirstPersonCameraService();
            _secondPersonCameraService = new SecondPersonCameraService();
            _thirdPersonCameraService = new ThirdPersonCameraService(_mainBattleCamera);
        }

        public override void OnAwake()
        {
            _switchViewEvents = World.Filter.With<SwitchPersonViewEvent>().Build();
            _localPlayersFilter = World.Filter.With<Translation>().With<LocalPlayerMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _switchViewEvents)
            {
                ref var switchViewEvent = ref entityEvent.GetComponent<SwitchPersonViewEvent>();

                foreach (var localPlayer in _localPlayersFilter)
                {
                    SwitchViewForPlayer(switchViewEvent, localPlayer);
                }
            }
        }

        private void SwitchViewForPlayer(SwitchPersonViewEvent switchViewEvent, Entity localPlayer)
        {
            ref var translation = ref localPlayer.GetComponent<Translation>();
            var localPlayerObject = translation.Transform.gameObject;

            if (localPlayerObject.TryGetComponent(out ChangePersonViewController changePersonViewController))
            {
                switch (switchViewEvent.ViewType)
                {
                    case EPersonViewType.First:
                        InputControls.SetFirstPersonMode();
                        changePersonViewController.SwitchToFirstPersonView();
                        _firstPersonCameraService.SwitchToFirstPerson(changePersonViewController);
                        break;
                    case EPersonViewType.Second:
                        InputControls.SetFirstPersonMode();
                        changePersonViewController.SwitchToThirdPersonView();
                        _secondPersonCameraService.SwitchToSecondPerson(changePersonViewController);
                        break;
                    case EPersonViewType.Third:
                        InputControls.SetThirdPersonMode();
                        changePersonViewController.SwitchToThirdPersonView();
                        _thirdPersonCameraService.SwitchToThirdPerson(changePersonViewController);
                        break;
                    default: break;
                }
            }
        }
    }
}