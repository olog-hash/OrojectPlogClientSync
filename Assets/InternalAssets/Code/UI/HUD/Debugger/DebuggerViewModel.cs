using System;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.Core;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Debugger
{
    public class DebuggerViewModel: BaseViewModel, ILayer
    {
        public bool IsActive => _isActive;
        
        public Action<bool> OnShowHideChanged;
        public Action OnDebuggerUpdate;

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
        
        private bool _isActive;

        public DebuggerViewModel()
        {
            OnShowHideChanged = null;
        }

        public void DebuggerUpdate()
        {
            OnDebuggerUpdate?.Invoke();
        }
        
        public void ShowLayer()
        {
            _isActive = true;
            OnShowHideChanged?.Invoke(_isActive);
        }

        public void HideLayer()
        {
            _isActive = false;
            OnShowHideChanged?.Invoke(_isActive);
        }
    }
}