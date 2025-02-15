using System.Collections.Generic;
using ProjectOlog.Code.UI.Core;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.KillPanel.View
{
    public class KillBarView : AbstractScreen<KillBarViewModel>
    {
        [SerializeField] private Transform _slotsRoot;
        [SerializeField] private KillBarSlot _slotPrefab;
        private List<KillBarSlot> _slotsList = new List<KillBarSlot>();
        
        private KillBarViewModel _currentViewModel;

        protected override void OnBind(KillBarViewModel model)
        {
            _currentViewModel = model;
            _currentViewModel.OnKillMessageReceived += OnKillMessageReceived;
            _currentViewModel.OnKillMessageVisibilityChanged += OnKillMessageVisibilityChanged;

            _slotsList.Clear();
            for (int i = 0; i < KillBarViewModel.MAX_COUNT; i++)
            {
                var slotMessage = Instantiate(_slotPrefab, _slotsRoot);
                slotMessage.gameObject.SetActive(false);
                
                slotMessage.Clear();
                _slotsList.Add(slotMessage);
            }
        }

        protected override void OnUnbind(KillBarViewModel model)
        {
            _currentViewModel.OnKillMessageReceived -= OnKillMessageReceived;
            _currentViewModel.OnKillMessageVisibilityChanged -= OnKillMessageVisibilityChanged;
        }

        private void OnKillMessageReceived()
        {
            UpdateKillMessages();
        }

        private void OnKillMessageVisibilityChanged()
        {
            UpdateKillMessages();
        }
        
        private void UpdateKillMessages()
        {
            for (int i = 0; i < _slotsList.Count; i++)
            {
                _slotsList[i].gameObject.SetActive(false);
            }

            int limit = Mathf.Min(_currentViewModel.KillMessages.Count, _slotsList.Count);
            for (int i = 0; i < limit; i++)
            {
                var messageData = _currentViewModel.KillMessages[i];
                var messageObject = _slotsList[i];

                messageObject.Display(messageData);
                messageObject.gameObject.SetActive(messageData.IsVisible);
            }
        }
    }
}