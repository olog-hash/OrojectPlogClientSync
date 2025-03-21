using System;
using R3;

namespace ProjectOlog.Code.UI.HUD.Tab.Models
{
    /// <summary>
    /// Хранит общую информацию о текущем матче.
    /// Включает данные о имени комнаты, сервере, карте и игровом режиме.
    /// </summary>
    public class MatchInfoModel : IDisposable
    {
        public ReactiveProperty<string> RoomName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ServerName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> MapName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ModeName { get; } = new ReactiveProperty<string>();

        public void SetMatchInfo(string roomName, string serverName, string mapName, string modeName)
        {
            RoomName.Value = roomName;
            ServerName.Value = serverName;
            MapName.Value = mapName;
            ModeName.Value = modeName;
        }
        
        public void Dispose()
        {
            RoomName.Dispose();
            ServerName.Dispose();
            MapName.Dispose();
            ModeName.Dispose();
        }
    }
}