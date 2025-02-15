using System.Collections.Generic;
using ProjectOlog.Code.UI.Core;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.TabPanel.DefaultTabView
{
    public class DefaultTabView: AbstractScreen<TabViewModel>
    {
        [SerializeField] private Transform _slotsRoot;
        [SerializeField] private UserSlotView _slotPrefab;

        private List<UserSlotView> _slotViews = new List<UserSlotView>();
        private TabViewModel _currentViewModel;

        protected override void OnBind(TabViewModel model)
        {
            OnShowHideChanged(false);
            
            _currentViewModel = model;
            _currentViewModel.OnUsersListChanged += OnUsersListChanged;
            _currentViewModel.OnUserStateUpdate += OnUserStateUpdate;
            _currentViewModel.OnShowHideChanged += OnShowHideChanged;
            
            OnUsersListChanged();
        }

        protected override void OnUnbind(TabViewModel model)
        {
            _currentViewModel.OnUsersListChanged -= OnUsersListChanged;
            _currentViewModel.OnUserStateUpdate -= OnUserStateUpdate;
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;

            DestroySlots();
        }

        public void DestroySlots()
        {
            for (int i = 0; i < _slotViews.Count; i++)
            {
                Destroy(_slotViews[i].gameObject);
            }
            
            _slotViews.Clear();
        }

        public void OnUsersListChanged()
        {
            DestroySlots();

            for (int i = 0; i < _currentViewModel.UsersData.Count; i++)
            {
                var userData = _currentViewModel.UsersData[i];
                
                var slotView = Instantiate(_slotPrefab, _slotsRoot);
                slotView.gameObject.SetActive(true);
                
                slotView.Clear();
                slotView.Initialize(i + 1, userData);
                
                _slotViews.Add(slotView);
            }
        }

        public void OnUserStateUpdate()
        {
            int limit = Mathf.Min(_currentViewModel.UsersData.Count, _slotViews.Count);
            
            for (int i = 0; i < limit; i++)
            {
                var userData = _currentViewModel.UsersData[i];
                var slotView = _slotViews[i];
                
                slotView.UpdateSlot(userData);
            }
        }
        
        private void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }
    }
}