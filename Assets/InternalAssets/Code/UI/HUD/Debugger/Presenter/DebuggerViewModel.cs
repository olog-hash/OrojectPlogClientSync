using System;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.Core;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Debugger
{
    public class DebuggerViewModel : BaseViewModel
    {
        // Основные данные
        public float FrameRate;
        public Vector3 Position;
        public Vector3 Velocity;
        
        // Сетевые данные
        public uint CurrentServerTick;
        public uint CurrentLocalTick;
        public int UsersCount;
        public int NetworkEntities;
        public int Ping;
        
        // Данные пакетов
        public int BytesInPerSecond;
        public int PacketsInPerSecond;
        public int BytesOutPerSecond;
        public int PacketsOutPerSecond;
        public float PacketLoss;
    }
}