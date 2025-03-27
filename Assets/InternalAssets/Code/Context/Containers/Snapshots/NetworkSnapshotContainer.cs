using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.DataStorage.Core;
using ProjectOlog.Code.Network.Client;

namespace ProjectOlog.Code.Network.Profiles.Snapshots
{
    public sealed class NetworkSnapshotContainer : ISceneContainer
    {
        public const int MAX_BUFFER_LENGTH = NetworkTime.DEFAULT_TICK_RATE * 3;

        // Предварительное выделение ёмкости
        private readonly Dictionary<uint, NetworkSnapshot> _snapshots = new Dictionary<uint, NetworkSnapshot>(MAX_BUFFER_LENGTH); 
        private uint _oldestTick;
        private uint _newestTick;
        private bool _oldestTickValid; // Флаг для отслеживания валидности _oldestTick

        public int Length => _snapshots.Count;
        public uint NewestTick => _newestTick;
        public uint OldestTick 
        { 
            get 
            {
                if (!_oldestTickValid)
                {
                    RecalculateOldestTick();
                }
                return _oldestTick;
            }
        }

        public void Reset()
        {
            _snapshots.Clear();
            _oldestTick = 0;
            _newestTick = 0;
            _oldestTickValid = true;
        }

        private void RecalculateOldestTick()
        {
            if (_snapshots.Count > 0)
            {
                uint minTick = uint.MaxValue;
                foreach (var tick in _snapshots.Keys)
                {
                    if (tick < minTick)
                    {
                        minTick = tick;
                    }
                }
                _oldestTick = minTick;
            }
            _oldestTickValid = true;
        }

        public void AddSnapshot(NetworkSnapshot snapshot)
        {
            uint tick = snapshot.LastServerTick;
            
            _snapshots[tick] = snapshot;
            
            if (_snapshots.Count == 1)
            {
                _oldestTick = tick;
                _newestTick = tick;
                _oldestTickValid = true;
            }
            else
            {
                // Обновляем _newestTick
                if (tick > _newestTick)
                {
                    _newestTick = tick;
                }
                
                // Обновляем _oldestTick если новый тик меньше
                if (tick < _oldestTick)
                {
                    _oldestTick = tick;
                    _oldestTickValid = true;
                }
            }

            // Удаляем старые снапшоты, если превысили ёмкость
            while (_snapshots.Count > MAX_BUFFER_LENGTH)
            {
                // Если _oldestTick не валиден, пересчитываем его
                if (!_oldestTickValid)
                {
                    RecalculateOldestTick();
                }
                
                // Удаляем самый старый снапшот
                _snapshots.Remove(_oldestTick);
                
                // Помечаем _oldestTick как невалидный после удаления
                _oldestTickValid = false;
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