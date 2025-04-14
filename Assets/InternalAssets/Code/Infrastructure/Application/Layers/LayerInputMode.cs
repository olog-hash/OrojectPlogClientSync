namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Информация о действующем активном слое:
    /// будет ли заблокирован курсор,
    /// будет ли заблокировано вращение камеры,
    /// будет ли заблокировано перемещение.
    /// </summary>
    public readonly struct LayerInputMode
    {
        public bool IsCursorLocked { get; }
        public bool IsMouseControlEnabled { get; }
        public bool IsKeyControlEnabled { get; }

        private LayerInputMode(bool isCursorLocked, bool isMouseControlEnabled, bool isKeyControlEnabled)
        {
            IsCursorLocked = isCursorLocked;
            IsMouseControlEnabled = isMouseControlEnabled;
            IsKeyControlEnabled = isKeyControlEnabled;
        }

        public static LayerInputMode None => new LayerInputMode(true, false, false);
        public static LayerInputMode Freedom => new LayerInputMode(false, true, true);
        public static LayerInputMode Game => new LayerInputMode(true, true, true);
        public static LayerInputMode SelectedPanel => new LayerInputMode(false, false, false);
        public static LayerInputMode SelectedPanelFreeMove => new LayerInputMode(false, false, true);
        public static LayerInputMode SelectedPanelFreeLook => new LayerInputMode(false, true, false);
        public static LayerInputMode SelectedPanelFreeMoveLook => new LayerInputMode(false, true, true);
    }
}