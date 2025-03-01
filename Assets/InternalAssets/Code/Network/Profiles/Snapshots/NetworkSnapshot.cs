using System.Collections.Generic;
using ProjectOlog.Code.Network.Profiles.Snapshots.ObjectTransform;
using ProjectOlog.Code.Network.Profiles.Snapshots.PlayerTransform;

namespace ProjectOlog.Code.Network.Profiles.Snapshots
{
    /// <summary>
    /// Снимок данных игроков в N тик сервера.
    /// </summary>
    public class NetworkSnapshot
    {
        public uint LastServerTick { get; private set; }
        public double ServerTime { get; private set; }
        public Dictionary<byte, NetworkPlayerTransform> PlayersTransform { get; private set; }
        public Dictionary<ushort, NetworkObjectTransform> ObjectsTransform { get; private set; }

        public NetworkSnapshot(uint lastServerTick, double serverTime)
        {
            LastServerTick = lastServerTick;
            ServerTime = serverTime;
            
            PlayersTransform = new Dictionary<byte, NetworkPlayerTransform>();
            ObjectsTransform = new Dictionary<ushort, NetworkObjectTransform>();
        }
        
        public void AddPlayerTransform(byte playerId, NetworkPlayerTransform transform)
        {
            PlayersTransform[playerId] = transform;
        }

        public bool TryGetPlayerTransform(byte playerId, out NetworkPlayerTransform transform)
        {
            return PlayersTransform.TryGetValue(playerId, out transform);
        }
        
        public void AddObjectTransform(ushort objectID, NetworkObjectTransform transform)
        {
            ObjectsTransform[objectID] = transform;
        }

        public bool TryGetObjectTransform(ushort objectID, out NetworkObjectTransform transform)
        {
            return ObjectsTransform.TryGetValue(objectID, out transform);
        }
    }
}