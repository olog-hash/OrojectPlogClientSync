using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    // Модель для графики (детальная реализация примера)
    public class GraphicsSettingsModel : BaseSettingsModel
    {
        // Реактивные свойства для настроек графики
        public ReactiveProperty<int> MaxFPS { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<bool> AdaptiveMonitor { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> FullscreenMode { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> VSync { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<float> Gamma { get; } = new ReactiveProperty<float>(1.0f);

        public ReactiveProperty<int> QualityLevel { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> TextureResolution { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> GeometryQuality { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> LightingQuality { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> ShadowsQuality { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> ParticlesQuality { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> DrawingDistance { get; } = new ReactiveProperty<int>(1);

        // Опции для элементов UI
        public string[] QualityOptions { get; } = { "Низкое", "Среднее", "Высокое", "Ультра" };
        public string[] DetailOptions { get; } = { "Низкое", "Среднее", "Высокое" };
        public string[] FpsOptions { get; } = { "30", "60", "120", "144", "Неограничено" };

        // Значения по умолчанию
        private readonly int _defaultMaxFPS = 2;
        private readonly bool _defaultAdaptiveMonitor = false;
        private readonly bool _defaultFullscreenMode = true;
        private readonly bool _defaultVSync = false;
        private readonly float _defaultGamma = 1.0f;
        private readonly int _defaultQualityLevel = 2;
        private readonly int _defaultTextureResolution = 1;
        private readonly int _defaultGeometryQuality = 1;
        private readonly int _defaultLightingQuality = 1;
        private readonly int _defaultShadowsQuality = 1;
        private readonly int _defaultParticlesQuality = 1;
        private readonly int _defaultDrawingDistance = 1;

        public GraphicsSettingsModel()
        {
            // Подписываемся на изменения для отслеживания
            SetupChangeTracking();
            LoadSettings();
        }

        private void SetupChangeTracking()
        {
            // Простой и эффективный способ отслеживания изменений
            MaxFPS.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            AdaptiveMonitor.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            FullscreenMode.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            VSync.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Gamma.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            QualityLevel.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            TextureResolution.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            GeometryQuality.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            LightingQuality.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ShadowsQuality.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ParticlesQuality.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            DrawingDistance.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
        }

        private void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs (упрощенно)
            MaxFPS.Value = PlayerPrefs.GetInt("Graphics_MaxFPS", _defaultMaxFPS);
            AdaptiveMonitor.Value =
                PlayerPrefs.GetInt("Graphics_AdaptiveMonitor", _defaultAdaptiveMonitor ? 1 : 0) == 1;
            FullscreenMode.Value = PlayerPrefs.GetInt("Graphics_FullscreenMode", _defaultFullscreenMode ? 1 : 0) == 1;
            VSync.Value = PlayerPrefs.GetInt("Graphics_VSync", _defaultVSync ? 1 : 0) == 1;
            Gamma.Value = PlayerPrefs.GetFloat("Graphics_Gamma", _defaultGamma);
            QualityLevel.Value = PlayerPrefs.GetInt("Graphics_QualityLevel", _defaultQualityLevel);
            TextureResolution.Value = PlayerPrefs.GetInt("Graphics_TextureResolution", _defaultTextureResolution);
            GeometryQuality.Value = PlayerPrefs.GetInt("Graphics_GeometryQuality", _defaultGeometryQuality);
            LightingQuality.Value = PlayerPrefs.GetInt("Graphics_LightingQuality", _defaultLightingQuality);
            ShadowsQuality.Value = PlayerPrefs.GetInt("Graphics_ShadowsQuality", _defaultShadowsQuality);
            ParticlesQuality.Value = PlayerPrefs.GetInt("Graphics_ParticlesQuality", _defaultParticlesQuality);
            DrawingDistance.Value = PlayerPrefs.GetInt("Graphics_DrawingDistance", _defaultDrawingDistance);

            SetHasChanges(false);
        }

        public override void ApplySettings()
        {
            // Сохранение настроек
            PlayerPrefs.SetInt("Graphics_MaxFPS", MaxFPS.Value);
            PlayerPrefs.SetInt("Graphics_AdaptiveMonitor", AdaptiveMonitor.Value ? 1 : 0);
            PlayerPrefs.SetInt("Graphics_FullscreenMode", FullscreenMode.Value ? 1 : 0);
            PlayerPrefs.SetInt("Graphics_VSync", VSync.Value ? 1 : 0);
            PlayerPrefs.SetFloat("Graphics_Gamma", Gamma.Value);
            PlayerPrefs.SetInt("Graphics_QualityLevel", QualityLevel.Value);
            PlayerPrefs.SetInt("Graphics_TextureResolution", TextureResolution.Value);
            PlayerPrefs.SetInt("Graphics_GeometryQuality", GeometryQuality.Value);
            PlayerPrefs.SetInt("Graphics_LightingQuality", LightingQuality.Value);
            PlayerPrefs.SetInt("Graphics_ShadowsQuality", ShadowsQuality.Value);
            PlayerPrefs.SetInt("Graphics_ParticlesQuality", ParticlesQuality.Value);
            PlayerPrefs.SetInt("Graphics_DrawingDistance", DrawingDistance.Value);

            // Применение настроек к Unity
            QualitySettings.SetQualityLevel(QualityLevel.Value, true);
            Screen.fullScreen = FullscreenMode.Value;
            QualitySettings.vSyncCount = VSync.Value ? 1 : 0;

            SetHasChanges(false);
        }

        public override void ResetToDefault()
        {
            MaxFPS.Value = _defaultMaxFPS;
            AdaptiveMonitor.Value = _defaultAdaptiveMonitor;
            FullscreenMode.Value = _defaultFullscreenMode;
            VSync.Value = _defaultVSync;
            Gamma.Value = _defaultGamma;
            QualityLevel.Value = _defaultQualityLevel;
            TextureResolution.Value = _defaultTextureResolution;
            GeometryQuality.Value = _defaultGeometryQuality;
            LightingQuality.Value = _defaultLightingQuality;
            ShadowsQuality.Value = _defaultShadowsQuality;
            ParticlesQuality.Value = _defaultParticlesQuality;
            DrawingDistance.Value = _defaultDrawingDistance;

            SetHasChanges(true);
        }

        public override void Dispose()
        {
            base.Dispose();

            MaxFPS.Dispose();
            AdaptiveMonitor.Dispose();
            FullscreenMode.Dispose();
            VSync.Dispose();
            Gamma.Dispose();
            QualityLevel.Dispose();
            TextureResolution.Dispose();
            GeometryQuality.Dispose();
            LightingQuality.Dispose();
            ShadowsQuality.Dispose();
            ParticlesQuality.Dispose();
            DrawingDistance.Dispose();
        }
    }
}