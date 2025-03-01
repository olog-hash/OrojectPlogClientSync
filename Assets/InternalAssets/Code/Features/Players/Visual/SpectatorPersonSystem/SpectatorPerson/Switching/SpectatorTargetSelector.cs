using System.Collections.Generic;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching
{
    public class SpectatorTargetSelector
    {
        private Filter _alivePlayers;
        private readonly Queue<int> _viewedTargets; // Хранит ServerID уже просмотренных игроков
        
        public SpectatorTargetSelector()
        {
            _alivePlayers = World.Default.Filter.With<NetworkIdentity>().With<NetworkPlayer>().Without<DeadMarker>()
                .Without<LocalPlayerMarker>().Build();
            
            _viewedTargets = new Queue<int>();
        }

        public Entity SelectNextTarget()
        {
            // Проверяем есть ли вообще живые игроки
            if (_alivePlayers.IsEmpty()) return null;

            // Собираем ID всех текущих живых игроков
            var currentAliveIds = new HashSet<int>();
            foreach (var entity in _alivePlayers)
            {
                ref var networkIdentity = ref entity.GetComponent<NetworkIdentity>();
                currentAliveIds.Add(networkIdentity.ServerID);
            }

            // Если живых игроков нет - возвращаем null
            if (currentAliveIds.Count == 0) return null;

            // Ищем игрока которого еще не смотрели в этом цикле
            foreach (var entity in _alivePlayers)
            {
                ref var networkIdentity = ref entity.GetComponent<NetworkIdentity>();
                if (!_viewedTargets.Contains(networkIdentity.ServerID))
                {
                    _viewedTargets.Enqueue(networkIdentity.ServerID);
                    return entity;
                }
            }

            // Если все живые игроки просмотрены - начинаем новый цикл
            _viewedTargets.Clear();
            return SelectNextTarget();
        }
        
        public void Reset()
        {
            _viewedTargets.Clear();
        }
    }
}