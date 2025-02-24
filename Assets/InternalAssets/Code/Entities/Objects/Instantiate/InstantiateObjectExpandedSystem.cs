using LiteNetLib.Utils;
using ProjectOlog.Code.Entities.Objects.Interactables;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Entities.Objects.Instantiate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InstantiateObjectExpandedSystem : TickrateSystem
    {
        private Filter _initObjectFilter;
        
        public override void OnAwake()
        {
            _initObjectFilter = World.Filter.With<InstantiateObjectEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initObjectFilter)
            {
                ref var instantiateObjectEvent = ref entityEvent.GetComponent<InstantiateObjectEvent>();
                ref var mapping = ref instantiateObjectEvent.EntityProviderMappingPool;
                var packet = instantiateObjectEvent.InstantiateObjectPacket;

                ProcessTransfer(packet.TransferObjectDatas, ref mapping);
                ProcessInteractable(packet.InteractionObjectDatas, ref mapping);
            }
        }
        
        private void ProcessTransfer(TransferObjectData[] transferObjectDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var transferObject in transferObjectDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(transferObject.EventID, out var provider))
                {
                    provider.Entity.SetComponent(new Transfer
                    {
                        LinearVelocity = transferObject.LinearVelocity,
                        AngularVelocity = transferObject.AngularVelocity,
                    });
                }
            }
        }
        
        private void ProcessInteractable(InteractionObjectData[] interactionObjectDatas, ref EntityProviderMappingPool mapping)
        {
            foreach (var interactionObject in interactionObjectDatas)
            {
                if (mapping.EventIDToEntityProvider.TryGetValue(interactionObject.EventID, out var provider))
                {
                    if (provider.Entity.Has<InteractionObjectComponent>())
                    {
                        ref var interactiveObjectComponent = ref provider.Entity.GetComponent<InteractionObjectComponent>();

                        interactiveObjectComponent.ObjectStateManager.Init();
                        interactiveObjectComponent.ObjectStateManager.SetSnapshotData(interactionObject.SnapshotObjectData);
                    }
                }
            }
        }
    }
}