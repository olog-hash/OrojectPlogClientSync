using LiteNetLib.Utils;
using ProjectOlog.Code.Entities.Objects.Interactables;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using ProjectOlog.Code.Input.PlayerInput.FirstPerson;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Objects;
using ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.SyncObjectActions
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SyncPlayerInteractionObjectSystem : TickrateSystem
    {
        private Filter _localPlayerFilter;

        private InteractionViewModel _interactionViewModel;
        private InteractiveObjectNetworker _interactiveObjectNetworker;
        
        public SyncPlayerInteractionObjectSystem(InteractiveObjectNetworker objectNetworker, InteractionViewModel interactionViewModel)
        {
            _interactiveObjectNetworker = objectNetworker;
            _interactionViewModel = interactionViewModel;
        }

        public override void OnAwake() 
        {
            _localPlayerFilter = World.Filter.With<FirstPersonInputs>().With<ShotOrigin>().Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _localPlayerFilter)
            {
                var playerInputs = entity.GetComponent<FirstPersonInputs>();
                var shotOrigin = entity.GetComponent<ShotOrigin>();
                bool isDead = entity.Has<DeadMarker>();

                if (!isDead && Physics.Raycast(shotOrigin.ShotOriginTranslation.position, shotOrigin.ShotOriginTranslation.Transform.forward, out RaycastHit hit, 5f))
                {
                    var targetObject = hit.collider.gameObject;
                    var tochableObjectMarker = targetObject.GetComponent<TochableObjectMarker>();
                    
                    if (tochableObjectMarker != null)
                    {
                        var entityProvider = tochableObjectMarker.MainObject.GetComponent<EntityProvider>();

                        if (entityProvider.Has<InteractionObjectComponent>() &&
                            entityProvider.Has<NetworkIdentity>())
                        {
                            ref var networkIdentity = ref entityProvider.Entity.GetComponent<NetworkIdentity>();
                            ref var interactionObjectComponent =
                                ref entityProvider.Entity.GetComponent<InteractionObjectComponent>();

                            if (interactionObjectComponent.ObjectStateManager.IsAvaliableToInteract())
                            {
                                _interactionViewModel.IsVisible = true;
                                // Название обьекта
                                _interactionViewModel.InteractionObjectName = interactionObjectComponent
                                    .ObjectStateManager.InteractionObjectName;
                                // Описание обьекта
                                _interactionViewModel.InteractionObjectDescription = interactionObjectComponent
                                    .ObjectStateManager.InteractionObjectDescription;
                                // Название действия.
                                var actionName = interactionObjectComponent.ObjectStateManager.GetStateInteractName();
                                _interactionViewModel.InteractionActionName = $"F - {actionName}";

                                if (playerInputs.IsUseLocked)
                                {
                                    // Отправляем запрос на сервер - что этот обьект был использован
                                    var useObjectRequestPacket = new NetDataPackage(networkIdentity.ServerID);

                                    _interactiveObjectNetworker.UseInteractionObjectRequest(useObjectRequestPacket);
                                }
                            }
                            else
                            {
                                _interactionViewModel.IsVisible = false;  
                            }
                        }
                    }
                    else
                    {
                        _interactionViewModel.IsVisible = false;
                        _interactionViewModel.ClearInfoText();
                    }
                }
                else
                {
                    _interactionViewModel.IsVisible = false;
                    _interactionViewModel.ClearInfoText();
                }
            }
        }
    }
}