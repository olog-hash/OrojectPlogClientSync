using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Networking.Client;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots
{
    public class NetworkSnapshotContainer
    {
        public const int MAX_BUFFER_LENGTH = NetworkTime.DEFAULT_TICK_RATE * 30;

        private readonly Dictionary<uint, NetworkSnapshot> _snapshots;
        private uint _oldestTick;
        private uint _newestTick;

        public int Length => _snapshots.Count;
        public uint NewestTick => _newestTick;
        public uint OldestTick => _oldestTick;

        public NetworkSnapshotContainer()
        {
            _snapshots = new Dictionary<uint, NetworkSnapshot>();
            _oldestTick = 0;
            _newestTick = 0;
        }

        public void ClearContainer()
        {
            _snapshots.Clear();
            _oldestTick = 0;
            _newestTick = 0;
        }

        public void AddSnapshot(NetworkSnapshot snapshot)
        {
            uint tick = snapshot.LastServerTick;
    
            _snapshots[tick] = snapshot;
    
            if (_snapshots.Count == 1)
            {
                _oldestTick = tick;
                _newestTick = tick;
            }
            else
            {
                _newestTick = tick;
            }

            // Удаляем старые снапшоты, если превысили ёмкость
            while (_snapshots.Count > MAX_BUFFER_LENGTH)
            {
                _snapshots.Remove(_oldestTick);
        
                // Находим следующий минимальный тик
                _oldestTick = _snapshots.Keys.Min();
            }
        }

        public NetworkSnapshot GetSnapshot(uint tick)
        {
            return _snapshots.TryGetValue(tick, out var snapshot) ? snapshot : null;
        }

        public NetworkSnapshot GetLatestSnapshot()
        {
            return GetSnapshot(_newestTick);
        }
        
        public bool HasSnapshot(uint tick)
        {
            return _snapshots.ContainsKey(tick);
        }

        public bool TryGetSnapshot(uint serverTick, out NetworkSnapshot snapshot)
        {
            return _snapshots.TryGetValue(serverTick, out snapshot);
        }
    }
}