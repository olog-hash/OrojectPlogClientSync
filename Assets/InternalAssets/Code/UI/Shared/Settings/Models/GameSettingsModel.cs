namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class GameSettingsModel : BaseSettingsModel
    {
        public override void ApplySettings()
        {
            // Реализация применения настроек игры
            SetHasChanges(false);
        }
    
        public override void ResetToDefault()
        {
            // Реализация сброса настроек игры
            SetHasChanges(true);
        }
    }
}