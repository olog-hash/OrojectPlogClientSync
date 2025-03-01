namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    public readonly struct LayerInfo
    {
        public bool IsCursorLocked { get; }
        public bool IsMouseControlEnabled { get; }
        public bool IsKeyControlEnabled { get; }

        private LayerInfo(bool isCursorLocked, bool isMouseControlEnabled, bool isKeyControlEnabled)
        {
            IsCursorLocked = isCursorLocked;
            IsMouseControlEnabled = isMouseControlEnabled;
            IsKeyControlEnabled = isKeyControlEnabled;
        }

        public static LayerInfo None => new LayerInfo(true, false, false);
        public static LayerInfo Freedom => new LayerInfo(false, true, true);
        public static LayerInfo Game => new LayerInfo(true, true, true);
        public static LayerInfo SelectedPanel => new LayerInfo(false, false, false);
        public static LayerInfo SelectedPanelFreeMove => new LayerInfo(false, false, true);
        public static LayerInfo SelectedPanelFreeLook => new LayerInfo(false, true, false);
        public static LayerInfo SelectedPanelFreeMoveLook => new LayerInfo(false, true, true);
    }
}