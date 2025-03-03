using System;
using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Network.Packets.SystemSync.Send;

namespace ProjectOlog.Code.Network.Profiles.Snapshots.Fragmentation
{
    /// <summary>
    /// Класс для сбора и обработки фрагментов снапшотов с буферизацией по количеству
    /// </summary>
    public class SnapshotFragmentCollector
    {
        private Dictionary<uint, SnapshotFragment[]> _fragments = new Dictionary<uint, SnapshotFragment[]>();
        private SortedSet<uint> _snapshotIds = new SortedSet<uint>(); // Отсортированный набор ID снапшотов
        private const int MAX_SNAPSHOTS = 10; 
        
        public void AddFragment(SnapshotFragment fragment, float currentTime)
        {
            // Проверка корректности входных данных
            if (fragment == null || fragment.FragmentData == null)
                return;

            if (fragment.TotalFragments == 0 || fragment.FragmentIndex >= fragment.TotalFragments)
                return;

            uint id = fragment.SnapshotId;

            // Инициализация массива для нового снапшота
            if (!_fragments.ContainsKey(id))
            {
                // Проверяем, не переполнен ли буфер
                if (_fragments.Count >= MAX_SNAPSHOTS && MAX_SNAPSHOTS > 0)
                {
                    // Убедимся, что у нас есть снапшоты для удаления
                    if (_snapshotIds.Count > 0)
                    {
                        // Удаляем снапшот с наименьшим ID (предположительно самый старый)
                        uint oldestId = _snapshotIds.Min;
                        _fragments.Remove(oldestId);
                        _snapshotIds.Remove(oldestId);
                    }
                }

                _fragments[id] = new SnapshotFragment[fragment.TotalFragments];
                _snapshotIds.Add(id);
            }

            // Сохраняем фрагмент в соответствующей позиции
            _fragments[id][fragment.FragmentIndex] = fragment;
        }
        
        // Проверяет, полностью ли собран снапшот с указанным ID
        public bool IsSnapshotComplete(uint snapshotId)
        {
            if (!_fragments.ContainsKey(snapshotId))
                return false;

            var fragmentArray = _fragments[snapshotId];
            if (fragmentArray == null || fragmentArray.Length == 0)
                return false;

            // Проверяем, что все фрагменты получены и не null
            return fragmentArray.All(fragment => fragment != null && fragment.FragmentData != null);
        }
        
        // Возвращает собранные данные снапшота и удаляет его из коллектора
        public byte[] GetCompleteSnapshot(uint snapshotId)
        {
            if (!IsSnapshotComplete(snapshotId))
                return null;

            var fragmentArray = _fragments[snapshotId];

            // Вычисляем общий размер
            int totalSize = fragmentArray.Sum(f => f.FragmentData.Length);
            if (totalSize <= 0)
                return new byte[0];

            byte[] result = new byte[totalSize];

            // Копируем данные из всех фрагментов
            int offset = 0;
            foreach (var fragment in fragmentArray)
            {
                if (fragment != null && fragment.FragmentData != null)
                {
                    Buffer.BlockCopy(fragment.FragmentData, 0, result, offset, fragment.FragmentData.Length);
                    offset += fragment.FragmentData.Length;
                }
            }

            // Удаляем собранный снапшот из хранилища
            _fragments.Remove(snapshotId);
            _snapshotIds.Remove(snapshotId);

            return result;
        }
        
        public int SnapshotCount => _fragments.Count;
        
        public void Clear()
        {
            _fragments.Clear();
            _snapshotIds.Clear();
        }
    }
}