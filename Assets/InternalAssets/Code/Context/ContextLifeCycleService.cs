using ProjectOlog.Code.DataStorage.Core;
using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.ResourceManagement;
using ProjectOlog.Code.Network.Client;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Context
{
    public class ContextLifeCycleService
    {
        private ContainersReloadService _containersReloadService;
        
        private ApplicationLayersController _applicationLayersController;
        private LayersManager _layersManager;

        public ContextLifeCycleService(ContainersReloadService containersReloadService, ApplicationLayersController applicationLayersController, LayersManager layersManager)
        {
            _containersReloadService = containersReloadService;
            
            _applicationLayersController = applicationLayersController;
            _layersManager = layersManager;
        }

        /// <summary>
        /// Производит очистку фундаментальных сервисов, контейнеров и утилит, которые будут актуальны на все время работы приложения
        /// </summary>
        public void ResetProjectContext()
        {
            InputControls.Reset();
            
            _applicationLayersController.Reset();
            
            _layersManager.Reset();

            PrefabsResourcesLoader.Reset();
            PrefabsResourcesLoader.RegisterAllPrefabs("Prefabs");
            
            NetworkTime.Reset();
            NetworkObjectRegistration.RegisterNetworkObjects();
            
            // Очищаем все контейнеры - как глобальные (проектные), так и контейнеры сцен.
            _containersReloadService.ResetAllContainers();
        }
        
        /// <summary>
        /// Производит очистку фундаментальных сервисов, контейнеров и утилит, которые будут актуальны на время игровой сцены (комнаты, битвы)
        /// </summary>
        public void ResetSceneContext()
        {
            NetworkTime.Reset();
            
            // Очищаем контейнеры сцен
            _containersReloadService.ResetSceneContainers();
        }
    }
}