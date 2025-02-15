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
        private const string LAYER_NAME = "BattleInventory";
        
        private InventoryViewModel _inventoryViewModel;
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public InventoryLogicSystem(InventoryViewModel inventoryViewModel, LocalPlayerMonitoring localPlayerMonitoring)
        {
            _inventoryViewModel = inventoryViewModel;
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {
            LayersManager.RegisterLayer(1, LAYER_NAME, _inventoryViewModel, LayerInfo.Freedom);
            
            _inventoryViewModel.OnHandleClose += OnHandleClose;
        }

        public override void Dispose()
        {
            LayersManager.RemoveLayer(LAYER_NAME);

            _inventoryViewModel.OnHandleClose -= OnHandleClose;
        }

        public override void OnUpdate(float deltaTime)
        {
           // _inventoryViewModel.OnUpdate(deltaTime);
           
            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                if (!LayersManager.IsLayerActive(LAYER_NAME) && LayersManager.IsLayerCanBeShown(LAYER_NAME))
                {
                    LayersManager.ShowLayer(LAYER_NAME);
                }
                else if (LayersManager.IsLayerActive(LAYER_NAME))
                {
                    LayersManager.HideLayer(LAYER_NAME);
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && LayersManager.IsLayerActive(LAYER_NAME))
            {
                LayersManager.HideLayer(LAYER_NAME);
            }

            if (_localPlayerMonitoring.IsDead())
            {
                if (LayersManager.IsLayerActive(LAYER_NAME))
                {
                    LayersManager.HideLayer(LAYER_NAME);
                }
            }
        }

        private void OnHandleClose()
        {
            if (LayersManager.IsLayerActive(LAYER_NAME))
            {
                LayersManager.HideLayer(LAYER_NAME);
            }
        }
    }
}