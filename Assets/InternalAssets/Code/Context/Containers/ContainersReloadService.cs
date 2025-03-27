using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.DataStorage.Core
{
    /// <summary>
    /// Класс который отвечает за своевременную очистку загруженных в приложение контейнеров.
    /// </summary>
    public class ContainersReloadService
    {
        private readonly List<ISceneContainer> _sceneContainers;
        private readonly List<IProjectContainer> _projectContainers;
        
        private readonly ContainersFactory _containersFactory;

        [Inject]
        public ContainersReloadService(ContainersFactory containersFactory)
        {
            _containersFactory = containersFactory;

            _sceneContainers = _containersFactory.GetAllSceneContainers();
            _projectContainers = _containersFactory.GetAllProjectContainers();
            
            Debug.Log($"Инициализирован ContainersService. " +
                      $"Найдено {_sceneContainers.Count} сцен-контейнеров и " +
                      $"{_projectContainers.Count} проект-контейнеров.");
        }
        
        // Сброс всех контейнеров
        public void ResetAllContainers()
        {
            // Сбросить все сцен-контейнеры
            foreach (var container in _sceneContainers)
            {
                container.Reset();
            }
    
            // Сбросить все проект-контейнеры
            foreach (var container in _projectContainers)
            {
                container.Reset();
            }
    
            Debug.Log("Все контейнеры сброшены");
        }
        
        // Сброс только сцен-контейнеров
        public void ResetSceneContainers()
        {
            foreach (var container in _sceneContainers)
            {
                container.Reset();
            }
            Debug.Log("Сцен-контейнеры сброшены");
        }
        
        // Сброс только проект-контейнеров
        public void ResetProjectContainers()
        {
            foreach (var container in _projectContainers)
            {
                container.Reset();
            }
            Debug.Log("Проект-контейнеры сброшены");
        }
        
        // Логирование информации о всех контейнерах
        private void LogAllContainers()
        {
            Debug.Log("Сцен-контейнеры:");
            foreach (var container in _sceneContainers)
            {
                Debug.Log($" - {container.GetType().Name}");
            }
            
            Debug.Log("Проект-контейнеры:");
            foreach (var container in _projectContainers)
            {
                Debug.Log($" - {container.GetType().Name}");
            }
        }
    }
}