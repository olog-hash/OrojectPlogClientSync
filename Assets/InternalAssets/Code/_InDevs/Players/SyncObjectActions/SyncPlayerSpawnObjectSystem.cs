using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Objects;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.SyncObjectActions
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SyncPlayerSpawnObjectSystem : TickrateSystem
    {
        private Filter _localPlayerFilter;

        private BasicObjectNetworker _objectNetworker;
        private LocalPlayerSession _localPlayerSession;

        public SyncPlayerSpawnObjectSystem(BasicObjectNetworker objectNetworker, LocalPlayerSession localPlayerSession)
        {
            _objectNetworker = objectNetworker;
            _localPlayerSession = localPlayerSession;
        }


        public override void OnAwake()
        {
            _localPlayerFilter = World.Filter.With<LocalPlayerMarker>().With<ShotOrigin>().With<FirstPersonInputs>().Without<DeadMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!LayersManager.IsLayerActive("Gameplay")) return;

            foreach (var entity in _localPlayerFilter)
            {
                var playerInputs = entity.GetComponent<FirstPersonInputs>();
                var shotOrigin = entity.GetComponent<ShotOrigin>();

                // Пропускаем если необходимая кнопка неактивна.
                if (!playerInputs.IsAltFireLocked) continue;

                if (UnityEngine.Physics.Raycast(shotOrigin.ShotOriginTranslation.position,
                        shotOrigin.ShotOriginTranslation.Transform.forward, out RaycastHit hit))
                {
                    // Вычисляем направление взгляда без вертикальной компоненты
                    Vector3 directionToLookAt = hit.point - shotOrigin.ShotOriginTranslation.position;
                    directionToLookAt.y = 0; // Убираем вертикальную компоненту, чтобы исключить наклон

                    // Создаем объект с новым направлением взгляда, сохраняя горизонтальную ориентацию
                    Quaternion lookRotation = Quaternion.LookRotation(directionToLookAt.normalized);

                    //Object.Instantiate(NetworkObjectRegistry.GetNetworkObjectPrefab(1), hit.point, lookRotation);
                    
                    // Отправляем данные
                    var spawnObjectRequestPacket = new NetDataPackage((byte)_localPlayerSession.CurrentSpawnObjectID, hit.point, lookRotation);
                    
                    _objectNetworker.SpawnObjectRequest(spawnObjectRequestPacket);
                }

            }
        }
    }
}