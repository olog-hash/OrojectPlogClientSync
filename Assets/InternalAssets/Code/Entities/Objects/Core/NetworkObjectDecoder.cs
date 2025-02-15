using System;
using LiteNetLib.Utils;
using ProjectOlog.Code.Entities.Objects.Interactables;
using ProjectOlog.Code.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Core
{
    public static class NetworkObjectDecoder
    {
        public static void DecodeObjectData(EntityProvider entityProvider, NetDataPackage dataPackage)
        {
            var entity = entityProvider.Entity;

            try
            {
                DecodeTransferComponent(entity, dataPackage);
                DecodeInteractionComponent(entity, dataPackage);
            }
            catch (Exception ex)
            {
                // Обработка исключений при чтении данных
                Console.WriteLine($"Ошибка при декодировании данных сетевого объекта: {ex.Message}");
            }
        }

        private static void DecodeTransferComponent(Entity entity, NetDataPackage dataPackage)
        {
            bool hasTransferComponent = dataPackage.GetBool();
            if (hasTransferComponent)
            {
                bool syncTransfer = dataPackage.GetBool();

                if (syncTransfer)
                {
                    Vector3 linearVelocity = dataPackage.GetVector3();
                    Vector3 angularVelocity = dataPackage.GetVector3();

                    entity.SetComponent(new Transfer
                    {
                        LinearVelocity = linearVelocity,
                        AngularVelocity = angularVelocity,
                    });
                }
            }
        }

        private static void DecodeInteractionComponent(Entity entity, NetDataPackage dataPackage)
        {
            bool hasInteractionComponent = dataPackage.GetBool();
            if (hasInteractionComponent)
            {
                ref var interactiveObjectComponent = ref entity.GetComponent<InteractionObjectComponent>();
                
                byte[] dataArray = dataPackage.GetByteArray();

                var snapshotDataPackage = new NetDataPackage();
                snapshotDataPackage.FromData(dataArray);

                interactiveObjectComponent.ObjectStateManager.Init();
                interactiveObjectComponent.ObjectStateManager.SetSnapshotData(snapshotDataPackage);
            }
        }
    }
}