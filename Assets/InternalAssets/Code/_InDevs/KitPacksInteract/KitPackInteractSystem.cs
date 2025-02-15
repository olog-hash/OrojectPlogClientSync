using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Zenject;

namespace ProjectOlog.Code._InDevs.KitPacksInteract
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class KitPackInteractSystem : TickrateSystem
    {
        private Filter _interactFilter;

        public override void OnAwake()
        {
            _interactFilter = World.Filter.With<KitPackInteractEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _interactFilter)
            {
                ref var interactEvent = ref entityEvent.GetComponent<KitPackInteractEvent>();

                KitPackInteract(interactEvent);
            }
        }

        private void KitPackInteract(KitPackInteractEvent kitPackInteractEvent)
        {
            //Debug.Log($"{kitPackInteractEvent.VictimEntity.GetComponent<NetworkIdentity>().ServerID}");
        }
    }
}