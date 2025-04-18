﻿using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.FirstPersonController;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.RespawnKinematicController
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnKinematicControllerSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;

        public override void OnAwake()
        {
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                ResetKinematicControllerState(spawnEvent);
            }
        }

        public void ResetKinematicControllerState(RespawnPlayerEvent respawnEvent)
        {
            var playerProvider = respawnEvent.PlayerProvider;
            if (playerProvider is null || !playerProvider.Has<LocalPlayerMarker>()) return;
            if (playerProvider.Has<TransitionStateRequestComponent>()) return;
            
            // Оставляем метку, чтобы игрок перешел в обычное состояние
            var transitionStateRequest = new TransitionStateRequestComponent()
            {
                IsActive = true,
                NextCharacterState = CharacterState.GroundMove,
            };
            
            playerProvider.AddComponentData(transitionStateRequest);
        }
    }
}