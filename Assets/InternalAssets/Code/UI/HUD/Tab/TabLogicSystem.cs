using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Tab
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TabLogicSystem : UpdateSystem
    {
        private TabViewModel _tabViewModel;

        private LayersManager _layersManager;

        public TabLogicSystem(TabViewModel tabViewModel, LayersManager layersManager)
        {
            _tabViewModel = tabViewModel;
            _layersManager = layersManager;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            //_tabViewModel.OnUpdate(deltaTime);
            if (UnityEngine.Input.GetKey(KeyCode.Tab))
            {
                _layersManager.ShowLayer(_tabViewModel.LayerName);
                if (!_layersManager.IsLayerActive(_tabViewModel.LayerName) && _layersManager.IsLayerCanBeShown(_tabViewModel.LayerName))
                {
                    _layersManager.ShowLayer(_tabViewModel.LayerName);
                }
            }
            else
            {
                if (_layersManager.IsLayerActive(_tabViewModel.LayerName))
                {
                    _layersManager.HideLayer(_tabViewModel.LayerName);
                }
            }
        }
    }
}