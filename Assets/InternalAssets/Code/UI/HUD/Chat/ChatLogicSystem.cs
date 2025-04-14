using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.HUD.Chat.Presenter;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.ChatPanel
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ChatLogicSystem : UpdateSystem
    {
        private ChatViewModel _chatViewModel;
        private LayersManager _layersManager;

        public ChatLogicSystem(ChatViewModel chatViewModel, LayersManager layersManager)
        {
            _chatViewModel = chatViewModel;
            _layersManager = layersManager;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnUpdate(float deltaTime)
        {
            _chatViewModel.OnUpdate(deltaTime);
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                if (!_layersManager.IsLayerActive(_chatViewModel.LayerName) && _layersManager.IsLayerCanBeShown(_chatViewModel.LayerName))
                {
                    _layersManager.ShowLayer(_chatViewModel.LayerName);
                }
                else if (_layersManager.IsLayerActive(_chatViewModel.LayerName))
                {
                    _layersManager.HideLayer(_chatViewModel.LayerName);
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && _layersManager.IsLayerActive(_chatViewModel.LayerName))
            {
                _layersManager.HideLayer(_chatViewModel.LayerName);
            }
        }
    }
}