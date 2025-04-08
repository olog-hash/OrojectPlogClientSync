namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class AudioSettingsModel : BaseSettingsModel
    {
        public override void ApplySettings()
        {
            // Реализация применения настроек звука
            SetHasChanges(false);
        }
    
        public override void ResetToDefault()
        {
            // Реализация сброса настроек звука
            SetHasChanges(true);
        }
    }
}