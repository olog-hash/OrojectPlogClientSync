using ProjectOlog.Code.Infrastructure.Application.Layers;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.TabPanel
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TabLogicSystem : UpdateSystem
    {
        private const string LAYER_NAME = "BattleTab";
        
        private TabViewModel _tabViewModel;

        public TabLogicSystem(TabViewModel tabViewModel)
        {
            _tabViewModel = tabViewModel;
        }

        public override void OnAwake()
        {
            LayersManager.RegisterLayer(1, LAYER_NAME, _tabViewModel, LayerInfo.Game);
        }
        
        public override void Dispose()
        {
            LayersManager.RemoveLayer(LAYER_NAME);
        }

        public override void OnUpdate(float deltaTime)
        {
            //_tabViewModel.OnUpdate(deltaTime);
            if (UnityEngine.Input.GetKey(KeyCode.Tab))
            {
                if (!LayersManager.IsLayerActive(LAYER_NAME) && LayersManager.IsLayerCanBeShown(LAYER_NAME))
                {
                    LayersManager.ShowLayer(LAYER_NAME);
                }
            }
            else
            {
                if (LayersManager.IsLayerActive(LAYER_NAME))
                {
                    LayersManager.HideLayer(LAYER_NAME);
                }
            }
        }
    }
}