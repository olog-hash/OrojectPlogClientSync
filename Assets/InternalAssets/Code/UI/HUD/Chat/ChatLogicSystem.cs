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
        private const string LAYER_NAME = "BattleChat";
        
        private ChatViewModel _chatViewModel;

        public ChatLogicSystem(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }

        public override void OnAwake()
        {
            LayersManager.RegisterLayer(1, LAYER_NAME, _chatViewModel, LayerInfo.SelectedPanel);
        }
        
        public override void Dispose()
        {
            LayersManager.RemoveLayer(LAYER_NAME);
        }

        public override void OnUpdate(float deltaTime)
        {
            _chatViewModel.OnUpdate(deltaTime);
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
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
        }
    }
}