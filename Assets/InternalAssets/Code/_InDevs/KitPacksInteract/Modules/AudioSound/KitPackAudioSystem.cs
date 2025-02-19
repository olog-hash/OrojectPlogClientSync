using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Replenish.Events;
using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.KitPacksInteract.Modules.AudioSound
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class KitPackAudioSystem : TickrateSystem
    {
        private Filter _healthKitPackFilter;
        private Filter _armorKitPackFilter;

        public override void OnAwake()
        {
            _healthKitPackFilter = World.Filter
                .With<EntityAggressorEvent>()
                .With<ReplenishHealthEvent>()
                .With<EntityVictimEvent>().Build();
            
            _armorKitPackFilter = World.Filter
                .With<EntityAggressorEvent>()
                .With<ReplenishArmorEvent>()
                .With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _healthKitPackFilter)
            {
                ref var agressorEvent = ref entityEvent.GetComponent<EntityAggressorEvent>();
                ref var victimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();

                if (agressorEvent.AggressorEntity.Has<KitPackAudio>()
                    && victimEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    KitPackInteract(agressorEvent.AggressorEntity);
                }
            }
            
            foreach (var entityEvent in _armorKitPackFilter)
            {
                ref var agressorEvent = ref entityEvent.GetComponent<EntityAggressorEvent>();
                ref var victimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();

                if (agressorEvent.AggressorEntity.Has<KitPackAudio>()
                    && victimEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    KitPackInteract(agressorEvent.AggressorEntity);
                }
            }
        }

        /// <summary>
        /// Проигрываем аудио-эффект если он есть на сущности
        /// </summary>
        private void KitPackInteract(Entity agressorEntity)
        {
            if (!TryGetComponent(agressorEntity, out KitPackAudio kitPackAudio)) return;

            if (kitPackAudio.AudioSource != null && kitPackAudio.AudioClip != null)
            {
                kitPackAudio.AudioSource?.PlayOneShot(kitPackAudio.AudioClip);
            }
        }
    }
}