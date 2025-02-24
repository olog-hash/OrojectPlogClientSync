﻿using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.Instantiate;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.CameraSystem.Player.ViewModes.Lifecycle
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InitSwitchPersonViewSystem : TickrateSystem
    {
        private Filter _initPlayersFilter;
        
        public override void OnAwake()
        {
            _initPlayersFilter = World.Filter.With<PostInstantiatePlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initPlayersFilter)
            {
                ref var postInstantiatePlayerEvent = ref entityEvent.GetComponent<PostInstantiatePlayerEvent>();
                
                InitPlayer(postInstantiatePlayerEvent);
            }
        }

        public void InitPlayer(PostInstantiatePlayerEvent postInstantiatePlayerEvent)
        {
            if (!postInstantiatePlayerEvent.PlayerEntity.Has<LocalPlayerMarker>()) return;
            
            World.CreateTickEvent(1).AddComponentData(new SwitchPersonViewEvent
            {
                ViewType = EPersonViewType.First
            });
        }
    }
}