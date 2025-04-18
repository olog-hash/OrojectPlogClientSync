using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Network.Profiles.Entities.Containers
{
     public class PlayerBaseEntityContainer : BaseEntityContainer
    {
        // Дополнительный индекс для быстрого поиска игроков по UserID
        private readonly Dictionary<byte, EntityProvider> _playersByUserId = new Dictionary<byte, EntityProvider>();

        public override void Clear()
        {
            base.Clear();
            _playersByUserId.Clear();
        }

        public override void AddEntity(EntityProvider entityProvider)
        {
            if (!IsAvaliableToAdd(entityProvider))
                return;

            // Добавляем в базовое хранилище
            base.AddEntity(entityProvider);
            
            // Добавляем в индекс пользователей
            ref var networkPlayer = ref entityProvider.Entity.GetComponent<NetworkPlayer>();
            _playersByUserId[networkPlayer.UserID] = entityProvider;
        }

        public EntityProvider GetPlayerEntity(byte userId)
        {
            return _playersByUserId.TryGetValue(userId, out var player) ? player : null;
        }

        public bool RemovePlayerEntity(byte userId)
        {
            if (!_playersByUserId.TryGetValue(userId, out var player))
                return false;
                
            // Получаем ServerID для удаления из основного хранилища
            ref var networkIdentity = ref player.Entity.GetComponent<NetworkIdentity>();
            ushort serverId = networkIdentity.ServerID;
            
            // Удаляем из обоих словарей
            _playersByUserId.Remove(userId);
            EntitiesById.Remove(serverId);
            
            return true;
        }

        public override bool RemoveNetworkEntity(ushort serverId)
        {
            var entity = GetNetworkEntity(serverId);
            if (entity == null)
                return false;
                
            // Удаляем также из индекса по userId
            ref var networkPlayer = ref entity.Entity.GetComponent<NetworkPlayer>();
            _playersByUserId.Remove(networkPlayer.UserID);
            
            // Удаляем из базового хранилища
            return base.RemoveNetworkEntity(serverId);
        }

        public bool TryGetPlayerEntity(byte userId, out EntityProvider entityProvider)
        {
            return _playersByUserId.TryGetValue(userId, out entityProvider);
        }

        public override bool IsAvaliableToAdd(EntityProvider entityProvider)
        {
            return entityProvider.Has<NetworkIdentity>() && entityProvider.Has<NetworkPlayer>();
        }
        
        // Проверка наличия игрока с конкретным UserID
        public bool ContainsPlayerWithUserId(byte userId) => _playersByUserId.ContainsKey(userId);
        
        // Получение всех UserID как массив
        public byte[] GetAllUserIds() => _playersByUserId.Keys.ToArray();
    }
}