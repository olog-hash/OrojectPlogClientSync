using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class GraphicsSettingsModel : BaseSettingsModel
    {
        // Реактивные свойства для настроек графики
        public ReactiveProperty<int> MaxFPS { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> ScreenResolution { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> VSync { get; } = new ReactiveProperty<int>(0);
        
        public ReactiveProperty<int> QualityLevel { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> RenderScale { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> ShadowsActive { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> ShadowsResolution { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> ShadowsDistance { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> AntiAliasing { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> TextureResolution { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> LevelOfDetail { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<int> AnisotropicFiltering { get; } = new ReactiveProperty<int>(1);

        // Опции для элементов UI
        public string[] VSyncOptions { get; } = { "Вкл", "Выкл" };
        public string[] QualityOptions { get; } = { "Низкое", "Среднее", "Высокое", "Ультра" };
        public string[] DetailOptions { get; } = { "Низкое", "Среднее", "Высокое" };
        public string[] FpsOptions { get; } = { "30", "60", "120", "144", "Неограничено" };
        public string[] ResolutionOptions { get; private set; } = { "1280x720", "1920x1080", "2560x1440", "3840x2160" };
        public string[] AntiAliasingOptions { get; } = { "Выкл", "2x MSAA", "4x MSAA", "8x MSAA" };
        public string[] AnisotropicOptions { get; } = { "Выкл", "2x", "4x", "8x", "16x" };
        public string[] RenderScaleOptions { get; } = { "50%", "75%", "100%", "125%", "150%" };
        public string[] ShadowsActiveOptions { get; } = { "Отключены", "Включены" };

        public string[] ShadowsDistanceOptions { get; } =
            { "Близко", "Средне", "Далеко", "Очень далеко", "Максимально" };

        public string[] LevelOfDetailOptions { get; } =
            { "Низкое", "Среднее", "Высокое", "Очень высокое", "Максимальное" };

        // Значения по умолчанию
        private readonly int _defaultMaxFPS = 2;
        private readonly int _defaultScreenResolution = 1;
        private readonly int _defaultVSync = 1;
        private readonly int _defaultQualityLevel = 2;
        private readonly int _defaultRenderScale = 2;
        private readonly int _defaultShadowsActive = 1;
        private readonly int _defaultShadowsResolution = 1;
        private readonly int _defaultShadowsDistance = 2;
        private readonly int _defaultAntiAliasing = 1;
        private readonly int _defaultTextureResolution = 1;
        private readonly int _defaultLevelOfDetail = 2;
        private readonly int _defaultAnisotropicFiltering = 1;

        public GraphicsSettingsModel()
        {
            // Инициализация доступных разрешений экрана
            InitializeResolutions();

            // Подписываемся на изменения для отслеживания
            SetupChangeTracking();
            LoadSettings();
        }

        private void InitializeResolutions()
        {
            // Получение списка доступных разрешений
            // Для примера используем фиксированный список
            ResolutionOptions = new string[]
            {
                "1280x720",
                "1920x1080",
                "2560x1440",
                "3840x2160"
            };
        }

        private void SetupChangeTracking()
        {
            // Подписка на все изменения чтобы отслеживать изменения настроек
            MaxFPS.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ScreenResolution.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            VSync.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            QualityLevel.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            RenderScale.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ShadowsActive.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ShadowsResolution.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            ShadowsDistance.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            AntiAliasing.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            TextureResolution.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            LevelOfDetail.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            AnisotropicFiltering.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
        }

        private void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs
            MaxFPS.Value = PlayerPrefs.GetInt("Graphics_MaxFPS", _defaultMaxFPS);
            ScreenResolution.Value = PlayerPrefs.GetInt("Graphics_ScreenResolution", _defaultScreenResolution);
            VSync.Value = PlayerPrefs.GetInt("Graphics_VSync", _defaultVSync);
            QualityLevel.Value = PlayerPrefs.GetInt("Graphics_QualityLevel", _defaultQualityLevel);
            RenderScale.Value = PlayerPrefs.GetInt("Graphics_RenderScale", _defaultRenderScale);
            ShadowsActive.Value = PlayerPrefs.GetInt("Graphics_ShadowsActive", _defaultShadowsActive);
            ShadowsResolution.Value = PlayerPrefs.GetInt("Graphics_ShadowsResolution", _defaultShadowsResolution);
            ShadowsDistance.Value = PlayerPrefs.GetInt("Graphics_ShadowsDistance", _defaultShadowsDistance);
            AntiAliasing.Value = PlayerPrefs.GetInt("Graphics_AntiAliasing", _defaultAntiAliasing);
            TextureResolution.Value = PlayerPrefs.GetInt("Graphics_TextureResolution", _defaultTextureResolution);
            LevelOfDetail.Value = PlayerPrefs.GetInt("Graphics_LevelOfDetail", _defaultLevelOfDetail);
            AnisotropicFiltering.Value =
                PlayerPrefs.GetInt("Graphics_AnisotropicFiltering", _defaultAnisotropicFiltering);

            SetHasChanges(false);
        }

        public override void ApplySettings()
        {
            // Сохранение настроек в PlayerPrefs
            PlayerPrefs.SetInt("Graphics_MaxFPS", MaxFPS.Value);
            PlayerPrefs.SetInt("Graphics_ScreenResolution", ScreenResolution.Value);
            PlayerPrefs.SetInt("Graphics_VSync", VSync.Value);
            PlayerPrefs.SetInt("Graphics_QualityLevel", QualityLevel.Value);
            PlayerPrefs.SetInt("Graphics_RenderScale", RenderScale.Value);
            PlayerPrefs.SetInt("Graphics_ShadowsActive", ShadowsActive.Value);
            PlayerPrefs.SetInt("Graphics_ShadowsResolution", ShadowsResolution.Value);
            PlayerPrefs.SetInt("Graphics_ShadowsDistance", ShadowsDistance.Value);
            PlayerPrefs.SetInt("Graphics_AntiAliasing", AntiAliasing.Value);
            PlayerPrefs.SetInt("Graphics_TextureResolution", TextureResolution.Value);
            PlayerPrefs.SetInt("Graphics_LevelOfDetail", LevelOfDetail.Value);
            PlayerPrefs.SetInt("Graphics_AnisotropicFiltering", AnisotropicFiltering.Value);

            // Применение настроек к Unity
            QualitySettings.SetQualityLevel(QualityLevel.Value, true);
            QualitySettings.vSyncCount = VSync.Value == 0 ? 1 : 0;

            // Применение дополнительных настроек
            ApplyResolution();
            ApplyRenderScale();
            ApplyAntiAliasing();
            ApplyShadowSettings();
            ApplyTextureSettings();
            ApplyLevelOfDetail();

            SetHasChanges(false);
        }

        private void ApplyResolution()
        {
            if (ScreenResolution.Value >= 0 && ScreenResolution.Value < ResolutionOptions.Length)
            {
                string resolution = ResolutionOptions[ScreenResolution.Value];
                string[] parts = resolution.Split('x');
                if (parts.Length == 2)
                {
                    if (int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
                    {
                        Screen.SetResolution(width, height, Screen.fullScreen);
                    }
                }
            }
        }

        private void ApplyRenderScale()
        {
            // Преобразуем индекс в значение масштаба рендеринга
            float[] scaleValues = { 0.5f, 0.75f, 1.0f, 1.25f, 1.5f };
            int index = Mathf.Clamp(RenderScale.Value, 0, scaleValues.Length - 1);

            // Применяем масштаб рендеринга
            // Примечание: способ применения зависит от используемого рендерера
            // Для URP или HDRP нужно использовать их API
#if UNITY_URP
            // Пример для URP
            var data = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
            if (data != null)
            {
                data.renderScale = scaleValues[index];
            }
#endif
        }

        private void ApplyAntiAliasing()
        {
            // Конвертация индекса в реальное значение AA
            int aaValue = 0;
            switch (AntiAliasing.Value)
            {
                case 1:
                    aaValue = 2;
                    break;
                case 2:
                    aaValue = 4;
                    break;
                case 3:
                    aaValue = 8;
                    break;
                default:
                    aaValue = 0;
                    break;
            }

            QualitySettings.antiAliasing = aaValue;
        }

        private void ApplyShadowSettings()
        {
            // Включение/выключение теней
            if (ShadowsActive.Value == 1)
            {
                QualitySettings.shadows = ShadowQuality.Disable;
            }
            else
            {
                QualitySettings.shadows = ShadowQuality.All;

                // Настройка разрешения теней
                switch (ShadowsResolution.Value)
                {
                    case 0:
                        QualitySettings.shadowResolution = ShadowResolution.Low;
                        break;
                    case 1:
                        QualitySettings.shadowResolution = ShadowResolution.Medium;
                        break;
                    case 2:
                        QualitySettings.shadowResolution = ShadowResolution.High;
                        break;
                    default:
                        QualitySettings.shadowResolution = ShadowResolution.Medium;
                        break;
                }

                // Дистанция теней
                float[] distanceValues = { 50f, 100f, 150f, 200f, 300f };
                int index = Mathf.Clamp(ShadowsDistance.Value, 0, distanceValues.Length - 1);
                QualitySettings.shadowDistance = distanceValues[index];
            }
        }

        private void ApplyTextureSettings()
        {
            // Настройка разрешения текстур
            switch (TextureResolution.Value)
            {
                case 0:
                    QualitySettings.globalTextureMipmapLimit = 2;
                    break; // Низкое
                case 1:
                    QualitySettings.globalTextureMipmapLimit = 1;
                    break; // Среднее
                case 2:
                    QualitySettings.globalTextureMipmapLimit = 0;
                    break; // Высокое
                default:
                    QualitySettings.globalTextureMipmapLimit = 1;
                    break;
            }

            // Анизотропная фильтрация
            switch (AnisotropicFiltering.Value)
            {
                case 0:
                    QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Enable;
                    break;
                case 2:
                case 3:
                case 4:
                    QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
                    break;
                default:
                    QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Enable;
                    break;
            }
        }

        private void ApplyLevelOfDetail()
        {
            // Настройка уровня детализации (LOD Bias)
            float[] lodValues = { 0.5f, 0.75f, 1.0f, 1.5f, 2.0f };
            int index = Mathf.Clamp(LevelOfDetail.Value, 0, lodValues.Length - 1);
            QualitySettings.lodBias = lodValues[index];
        }

        public override void ResetToDefault()
        {
            MaxFPS.Value = _defaultMaxFPS;
            ScreenResolution.Value = _defaultScreenResolution;
            VSync.Value = _defaultVSync;
            QualityLevel.Value = _defaultQualityLevel;
            RenderScale.Value = _defaultRenderScale;
            ShadowsActive.Value = _defaultShadowsActive;
            ShadowsResolution.Value = _defaultShadowsResolution;
            ShadowsDistance.Value = _defaultShadowsDistance;
            AntiAliasing.Value = _defaultAntiAliasing;
            TextureResolution.Value = _defaultTextureResolution;
            LevelOfDetail.Value = _defaultLevelOfDetail;
            AnisotropicFiltering.Value = _defaultAnisotropicFiltering;

            SetHasChanges(true);
        }

        public override void Dispose()
        {
            base.Dispose();

            MaxFPS.Dispose();
            ScreenResolution.Dispose();
            VSync.Dispose();
            QualityLevel.Dispose();
            RenderScale.Dispose();
            ShadowsActive.Dispose();
            ShadowsResolution.Dispose();
            ShadowsDistance.Dispose();
            AntiAliasing.Dispose();
            TextureResolution.Dispose();
            LevelOfDetail.Dispose();
            AnisotropicFiltering.Dispose();
        }
    }
}