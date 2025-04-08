namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class ControlsSettingsModel : BaseSettingsModel
    {
        public override void ApplySettings()
        {
            // Реализация применения настроек управления
            SetHasChanges(false);
        }
    
        public override void ResetToDefault()
        {
            // Реализация сброса настроек управления
            SetHasChanges(true);
        }
    }
}