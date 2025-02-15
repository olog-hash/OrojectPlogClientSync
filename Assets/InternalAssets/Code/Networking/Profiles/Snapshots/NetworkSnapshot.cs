using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Networking.Profiles.Snapshots.NetworkTransformUtilits;
using ProjectOlog.Code.Networking.Profiles.Snapshots.ObjectTransform;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots
{
    /// <summary>
    /// Снимок данных игроков в N тик сервера.
    /// </summary>
    public class NetworkSnapshot
    {
        public uint LastServerTick { get; private set; }
        public double ServerTime { get; private set; }
        public Dictionary<int, NetworkPlayerTransform> PlayersTransform { get; private set; }
        public Dictionary<int, NetworkObjectTransform> ObjectsTransform { get; private set; }

        public NetworkSnapshot(uint lastServerTick, double serverTime)
        {
            LastServerTick = lastServerTick;
            ServerTime = serverTime;
            
            PlayersTransform = new Dictionary<int, NetworkPlayerTransform>();
            ObjectsTransform = new Dictionary<int, NetworkObjectTransform>();
        }
        
        public void AddPlayerTransform(int playerId, NetworkPlayerTransform transform)
        {
            PlayersTransform[playerId] = transform;
        }

        public bool TryGetPlayerTransform(int playerId, out NetworkPlayerTransform transform)
        {
            return PlayersTransform.TryGetValue(playerId, out transform);
        }
        
        public void AddObjectTransform(int objectID, NetworkObjectTransform transform)
        {
            ObjectsTransform[objectID] = transform;
        }

        public bool TryGetObjectTransform(int objectID, out NetworkObjectTransform transform)
        {
            return ObjectsTransform.TryGetValue(objectID, out transform);
        }
    }
}