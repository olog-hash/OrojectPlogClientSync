using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Infrastructure.Logging;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core
{
    public sealed class ConsoleCommandNetworker : NetWorkerClient
    {
        public override void Initialize()
        {
            CommandManager.RegisterCommand("spawn_request", SpawnRequestCommand, "Spawn me request", CommandType.normal);
            CommandManager.RegisterCommand("remove_player", RemovePlayerCommand, "Remove player by name", CommandType.normal);
            CommandManager.RegisterCommand("remove_object", RemoveObjectCommand, "Remove object by ID", CommandType.normal);
            CommandManager.RegisterCommand("remove_all_objects", RemoveAllObjectsCommand, "Remove all objects", CommandType.normal);
            CommandManager.RegisterCommand("add_shield_player", AddShieldToPlayer, "Add shield to played by time", CommandType.normal);
            CommandManager.RegisterCommand("set_delta_snapshot", ChangeDeltaSnapshotCommand, "Change mode of server snapshot sending (delta/global)", CommandType.normal);
        }

        private void SpawnRequestCommand(string[] args)
        {
            SendTo(nameof(SpawnRequestCommand), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        private void RemovePlayerCommand(string[] args)
        {
            if (args.Length > 0)
            {
                SendTo(nameof(RemovePlayerCommand), new NetDataPackage(args[0]), DeliveryMethod.ReliableOrdered);
            }
        }
        
        private void RemoveObjectCommand(string[] args)
        {
            if (args.Length > 0 && int.TryParse(args[0], out var ID))
            {
                SendTo(nameof(RemoveObjectCommand), new NetDataPackage(ID), DeliveryMethod.ReliableOrdered);
            }
        }
        
        private void RemoveAllObjectsCommand(string[] args)
        {
            SendTo(nameof(RemoveAllObjectsCommand), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        private void AddShieldToPlayer(string[] args)
        {
            if (args.Length >= 2 && int.TryParse(args[1], out var time))
            {
                SendTo(nameof(AddShieldToPlayer), new NetDataPackage(args[0], time), DeliveryMethod.ReliableOrdered);
            }
        }

        private void ChangeDeltaSnapshotCommand(string[] args)
        {
            if (args.Length >= 0 && byte.TryParse(args[0], out var value))
            {
                SendTo(nameof(ChangeDeltaSnapshotCommand), new NetDataPackage(value), DeliveryMethod.ReliableOrdered);
            }
        }
    }
}