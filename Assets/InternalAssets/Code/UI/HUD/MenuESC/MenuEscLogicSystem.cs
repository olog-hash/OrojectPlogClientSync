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

        public MenuEscLogicSystem(MenuEscViewModel menuEscViewModel)
        {
            _menuEscViewModel = menuEscViewModel;
        }

        public override void OnAwake()
        {
            // Регистрируем слой в LayersManager с высоким приоритетом
            LayersManager.RegisterLayer(2, MenuEscViewModel.LAYER_NAME, _menuEscViewModel, LayerInfo.SelectedPanel);
        }
        
        public override void Dispose()
        {
            LayersManager.RemoveLayer(MenuEscViewModel.LAYER_NAME);
        }

        public override void OnUpdate(float deltaTime)
        {
            // Обработка нажатия клавиши ESC для показа/скрытия меню
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (LayersManager.IsLayerActive(MenuEscViewModel.LAYER_NAME))
                {
                    LayersManager.HideLayer(MenuEscViewModel.LAYER_NAME);
                }
                else if (LayersManager.IsLayerCanBeShown(MenuEscViewModel.LAYER_NAME))
                {
                    LayersManager.ShowLayer(MenuEscViewModel.LAYER_NAME);
                }
            }
        }
    }
}