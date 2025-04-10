using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class AudioSettingsModel : BaseSettingsModel
    {
        // Реактивные свойства для настроек звука
        public ReactiveProperty<int> MuteAll { get; } = new ReactiveProperty<int>(1); // 0 - да, 1 - нет
        public ReactiveProperty<float> MasterVolume { get; } = new ReactiveProperty<float>(80); // от 0 до 100
        public ReactiveProperty<float> MusicVolume { get; } = new ReactiveProperty<float>(70); // от 0 до 100
        public ReactiveProperty<float> EffectsVolume { get; } = new ReactiveProperty<float>(75); // от 0 до 100

        // Опции для элементов UI
        public string[] MuteOptions { get; } = { "ДА", "НЕТ" };

        // Значения по умолчанию
        private readonly int _defaultMuteAll = 1; // По умолчанию "НЕТ"
        private readonly float _defaultMasterVolume = 80f;
        private readonly float _defaultMusicVolume = 70f;
        private readonly float _defaultEffectsVolume = 75f;

        public AudioSettingsModel()
        {
            // Инициализация
            SetupChangeTracking();
            LoadSettings();
        }

        private void SetupChangeTracking()
        {
            // Подписка на все изменения чтобы отслеживать изменения настроек
            MuteAll.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MasterVolume.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MusicVolume.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            EffectsVolume.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
        }

        private void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs
            MuteAll.Value = PlayerPrefs.GetInt("Audio_MuteAll", _defaultMuteAll);
            MasterVolume.Value = PlayerPrefs.GetFloat("Audio_MasterVolume", _defaultMasterVolume);
            MusicVolume.Value = PlayerPrefs.GetFloat("Audio_MusicVolume", _defaultMusicVolume);
            EffectsVolume.Value = PlayerPrefs.GetFloat("Audio_EffectsVolume", _defaultEffectsVolume);

            SetHasChanges(false);
        }

        public override void ApplySettings()
        {
            // Сохранение настроек в PlayerPrefs
            PlayerPrefs.SetInt("Audio_MuteAll", MuteAll.Value);
            PlayerPrefs.SetFloat("Audio_MasterVolume", MasterVolume.Value);
            PlayerPrefs.SetFloat("Audio_MusicVolume", MusicVolume.Value);
            PlayerPrefs.SetFloat("Audio_EffectsVolume", EffectsVolume.Value);
            
            // Применение настроек к аудиосистеме игры
            // TODO: Добавить реальную логику применения настроек звука
            
            SetHasChanges(false);
        }

        public override void ResetToDefault()
        {
            // Сброс настроек к значениям по умолчанию
            MuteAll.Value = _defaultMuteAll;
            MasterVolume.Value = _defaultMasterVolume;
            MusicVolume.Value = _defaultMusicVolume;
            EffectsVolume.Value = _defaultEffectsVolume;

            SetHasChanges(true);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            // Очистка реактивных свойств
            MuteAll.Dispose();
            MasterVolume.Dispose();
            MusicVolume.Dispose();
            EffectsVolume.Dispose();
        }
    }
}