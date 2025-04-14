using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.HUD.MenuESC.Presenter;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.SceneManagement;

namespace ProjectOlog.Code.UI.HUD.MenuESC
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MenuEscLogicSystem : UpdateSystem
    {
        private MenuEscViewModel _menuEscViewModel;
        private LayersManager _layersManager;

        public MenuEscLogicSystem(MenuEscViewModel menuEscViewModel, LayersManager layersManager)
        {
            _menuEscViewModel = menuEscViewModel;
            _layersManager = layersManager;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            // Обработка нажатия клавиши ESC для показа/скрытия меню
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_layersManager.IsLayerActive(_menuEscViewModel.LayerName))
                {
                    _layersManager.HideLayer(_menuEscViewModel.LayerName);
                }
                else if (_layersManager.IsLayerCanBeShown(_menuEscViewModel.LayerName))
                {
                    _layersManager.ShowLayer(_menuEscViewModel.LayerName);
                }
            }
        }
    }
}