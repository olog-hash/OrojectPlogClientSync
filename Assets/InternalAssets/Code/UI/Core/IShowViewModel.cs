using System;

namespace ProjectOlog.Code.UI.Core
{
    public interface IShowViewModel
    {
        public event Action<bool> OnShowHideChanged;

        public void OnShow();

        public void OnHide();
    }
}