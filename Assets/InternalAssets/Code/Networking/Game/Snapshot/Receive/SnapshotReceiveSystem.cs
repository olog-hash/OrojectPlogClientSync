using System.Collections.Generic;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using ProjectOlog.Code.Networking.Profiles.Snapshots;
using ProjectOlog.Code.Networking.Profiles.Snapshots.ObjectTransform;
using ProjectOlog.Code.Networking.Profiles.Snapshots.PlayerTransform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Networking.Game.Snapshot.Receive
{
    /// <summary>
    /// Получение и обработка снапшотов от сервера.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SnapshotReceiveSystem : TickrateSystem
    {
        private Filter _snapshotEventFilter;

        private NetworkEntitiesContainer _entitiesContainer;
        private NetworkSnapshotContainer _snapshotContainer;

        private NetworkPlayerTransformDeserializer _networkPlayerTransformDeserializer;
        private NetworkObjectTransformDeserializer _networkObjectTransformDeserializer;
        
        // Список для сортировки снапшотов
        private List<ServerSnapshotEvent> _sortedSnapshots = new List<ServerSnapshotEvent>();


        public SnapshotReceiveSystem(NetworkSnapshotContainer snapshotContainer, NetworkEntitiesContainer entitiesContainer)
        {
            _snapshotContainer = snapshotContainer;
            _entitiesContainer = entitiesContainer;

            _networkPlayerTransformDeserializer = new NetworkPlayerTransformDeserializer();
            _networkObjectTransformDeserializer = new NetworkObjectTransformDeserializer();
        }

        public override void OnAwake()
        {
            _snapshotEventFilter = World.Filter
                .With<ServerSnapshotEvent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Очищаем список
            _sortedSnapshots.Clear();

            // Собираем все снапшоты
            foreach (var eventEntity in _snapshotEventFilter)
            {
                ref var snapshotEvent = ref eventEntity.GetComponent<ServerSnapshotEvent>();
                _sortedSnapshots.Add(snapshotEvent);
            }

            // Сортируем по номеру тика
            _sortedSnapshots.Sort((a, b) => a.LastServerTick.CompareTo(b.LastServerTick));
            
            // Обрабатываем в правильном порядке
            foreach (var snapshotEvent in _sortedSnapshots)
            {
                NetworkTime.SetServerTick(snapshotEvent.LastServerTick);
                ProcessIncomingSnapshot(snapshotEvent);
            }
        }


        // Получает снапшот, вычисляет дельту и сохраняет в буфер
        private void ProcessIncomingSnapshot(ServerSnapshotEvent snapshotEvent)
        {
            var newSnapshot = new NetworkSnapshot(snapshotEvent.LastServerTick, snapshotEvent.LastServerTime);
            var previousSnapshot = _snapshotContainer.GetSnapshot(snapshotEvent.LastClientReceivedTick);
            
            ProcessPlayersData(snapshotEvent, ref newSnapshot, ref previousSnapshot);
            ProcessObjectsData(snapshotEvent, ref newSnapshot, ref previousSnapshot);
            
            _snapshotContainer.AddSnapshot(newSnapshot);
        }
        
        // Логика обработки для данных игрока
        private void ProcessPlayersData(ServerSnapshotEvent snapshotEvent, ref NetworkSnapshot snapshot, ref NetworkSnapshot previousSnapshot)
        {
            // Сначала обрабатываем новые данные из пакета
            var processedPlayerIds = new HashSet<int>();
            foreach (var playerData in snapshotEvent.PlayersTransform)
            {
                // Пытаемся получить предыдущий снапшот для этого игрока (для дельта-декодирования)
                NetworkPlayerTransform previousTransform = null;
                if (snapshotEvent.BroadcastType != ESnapshotBroadcastType.Global && previousSnapshot != null)
                {
                    previousSnapshot.TryGetPlayerTransform(playerData.UserID, out previousTransform);
                }

                // Десериализуем данные игрока
                var playerTransform = _networkPlayerTransformDeserializer.Deserialize(
                    previousTransform,
                    playerData.TransformData
                );

                // Добавляем в снапшот
                snapshot.AddPlayerTransform(playerData.UserID, playerTransform);
                processedPlayerIds.Add(playerData.UserID);
            }

            // Добавляем игроков которые есть на карте, но которые могли быть неподвижны в прошлом снапшоте.
            if (previousSnapshot != null)
            {
                foreach (var playerData in previousSnapshot.PlayersTransform)
                {
                    var playerID = playerData.Key;
                    var playerTransform = playerData.Value;
                    
                    // Если данные этого игрока уже обработаны из пакета - пропускаем
                    if (processedPlayerIds.Contains(playerID))
                        continue;

                    // Если он все еще существует на карте
                    if (_entitiesContainer.PlayerEntities.TryGetPlayerEntity(playerID, out var networkEntity))
                    {
                        var playerTransformClone = playerTransform.Clone();
                        
                        snapshot.AddPlayerTransform(playerID, playerTransformClone);
                    }
                }
            }
        }
        
        // Обрабатываем данные для обьектов
        private void ProcessObjectsData(ServerSnapshotEvent snapshotEvent, ref NetworkSnapshot snapshot, ref NetworkSnapshot previousSnapshot)
        {
            // Сначала обрабатываем новые данные из пакета
            var processedObjectIds = new HashSet<int>();
            foreach (var objectData in snapshotEvent.ObjectsTransform)
            {
                // Пытаемся получить предыдущий снапшот для этого обьекта (для дельта-декодирования)
                NetworkObjectTransform previousTransform = null;
                if (snapshotEvent.BroadcastType != ESnapshotBroadcastType.Global && previousSnapshot != null)
                {
                    previousSnapshot.TryGetObjectTransform(objectData.ServerID, out previousTransform);
                }

                // Десериализуем данные обьекта
                var objectTransform = _networkObjectTransformDeserializer.Deserialize(
                    previousTransform,
                    objectData.TransformData
                );

                // Добавляем в снапшот
                snapshot.AddObjectTransform(objectData.ServerID, objectTransform);
                processedObjectIds.Add(objectData.ServerID);
            }

            // Добавляем обьекты которые есть на карте, но которые могли быть неподвижны в прошлом снапшоте.
            if (previousSnapshot != null)
            {
                foreach (var objectData in previousSnapshot.ObjectsTransform)
                {
                    var serverID = objectData.Key;
                    var objectTranform = objectData.Value;
                    
                    // Если данные этого обьекта уже обработаны из пакета - пропускаем
                    if (processedObjectIds.Contains(serverID))
                        continue;

                    // Если он все еще существует на карте
                    if (_entitiesContainer.TryGetNetworkEntity(serverID, out var networkEntity))
                    {
                        snapshot.AddObjectTransform(serverID, objectTranform.Clone());
                    }
                }
            }
        }
    }
}