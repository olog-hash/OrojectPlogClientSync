using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.InventoryPanel
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InventoryLogicSystem : UpdateSystem
    {
        private InventoryViewModel _inventoryViewModel;
        private LayersManager _layersManager;
        
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public InventoryLogicSystem(InventoryViewModel inventoryViewModel, LocalPlayerMonitoring localPlayerMonitoring, LayersManager layersManager)
        {
            _inventoryViewModel = inventoryViewModel;
            _localPlayerMonitoring = localPlayerMonitoring;
            _layersManager = layersManager;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
           // _inventoryViewModel.OnUpdate(deltaTime);
           
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                if (!_layersManager.IsLayerActive(_inventoryViewModel.LayerName) && _layersManager.IsLayerCanBeShown(_inventoryViewModel.LayerName) &&
                    !_localPlayerMonitoring.IsDead())
                {
                    _layersManager.ShowLayer(_inventoryViewModel.LayerName);
                }
                else if (_layersManager.IsLayerActive(_inventoryViewModel.LayerName))
                {
                    _layersManager.HideLayer(_inventoryViewModel.LayerName);
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && _layersManager.IsLayerActive(_inventoryViewModel.LayerName))
            {
                _layersManager.HideLayer(_inventoryViewModel.LayerName);
            }

            if (_localPlayerMonitoring.IsDead())
            {
                if (_layersManager.IsLayerActive(_inventoryViewModel.LayerName))
                {
                    _layersManager.HideLayer(_inventoryViewModel.LayerName);
                }
            }
        }
    }
}