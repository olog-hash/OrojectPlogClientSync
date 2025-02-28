using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Mortality.Core;
using Scellecs.Morpeh;

namespace ProjectOlog.Code._InDevs.Data
{
    public class LocalPlayerMonitoring
    {
        private Filter _localPlayerFilter;

        public LocalPlayerMonitoring()
        {
            _localPlayerFilter = World.Default.Filter.With<LocalPlayerMarker>().Build();
        }

        public bool TryGetLocalPlayer(out Entity entity)
        {
            entity = _localPlayerFilter.FirstOrDefault();
            return entity != null && !entity.IsNullOrDisposed();
        }
        
        public bool IsDead()
        {
            return !TryGetLocalPlayer(out var entity) || entity.Has<DeadMarker>();
        }
    }
}