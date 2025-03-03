using ProjectOlog.Code.Engine.Cameras.Types;
using ProjectOlog.Code.Features.Entities.Destruction.ListDestroy;
using ProjectOlog.Code.Features.Entities.Destruction.SingleDestroy;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Network.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Battle.ECS.Rules.SimpleRules
{
    /// <summary>
    /// Правило, что переключает главную камеру на боевую камеру, в случаи если локальный игрок будет уничтожен.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [UpdateBefore(typeof(DestroyNetworkObjectSystem))]
    public sealed class ReturnBattleCamereAfterDestroyPlayerSystem : UpdateSystem
    {
        private Filter _destroyEventFilter;

        private readonly BattleCameraContainer _battleCameraContainer;
        private readonly NetworkEntitiesContainer _entitiesContainer;

        public ReturnBattleCamereAfterDestroyPlayerSystem(BattleCameraContainer battleCameraContainer, NetworkEntitiesContainer entitiesContainer)
        {
            _battleCameraContainer = battleCameraContainer;
            _entitiesContainer = entitiesContainer;
        }

        public override void OnAwake()
        {
            _destroyEventFilter = World.Filter.With<DestroyNetworkObjectEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _destroyEventFilter)
            {
                ref var destroyEvent = ref entityEvent.GetComponent<DestroyNetworkObjectEvent>();

                if (_entitiesContainer.TryGetNetworkEntity(destroyEvent.ServerID, out var playerProvider))
                {
                    // Если это локальный игрок - включаем боевую камеру.
                    if (playerProvider.Entity.Has<LocalPlayerMarker>())
                    {
                        _battleCameraContainer.ResetBattlePosition();
                    }
                }
            }
        }
    }
}