using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Custom;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    // Контроллер вкладки графики
    public class GraphicsTabController : UIToolkitElementView
    {
        // Контроллеры элементов
        private DropdownController _maxFpsDropdown;
        private ToggleController _adaptiveMonitorToggle;
        private ToggleController _fullscreenModeToggle;
        private ToggleController _vSyncToggle;
        private SliderController _gammaSlider;

        private CarouselController _qualityLevelCarousel;
        private CarouselController _textureResolutionCarousel;
        private CarouselController _geometryQualityCarousel;
        private CarouselController _lightingQualityCarousel;
        private CarouselController _shadowsQualityCarousel;
        private CarouselController _particlesQualityCarousel;
        private CarouselController _drawingDistanceCarousel;

        private Button _defaultSettingsButton;

        private GraphicsSettingsModel _model;

        public GraphicsTabController(VisualElement root) : base(root)
        {
        }

        protected override void SetVisualElements()
        {
            // Получение UI-элементов
            var maxFpsElement = Root.Q<VisualElement>("fps-lock");
            var adaptiveMonitorElement = Root.Q<VisualElement>("adaptive-monitor");
            var fullscreenModeElement = Root.Q<VisualElement>("fullscreen-mode");
            var vSyncElement = Root.Q<VisualElement>("vsync");
            var gammaElement = Root.Q<VisualElement>("gamma");

            var qualityLevelElement = Root.Q<VisualElement>("quality-level");
            var textureResolutionElement = Root.Q<VisualElement>("texture-resolution");
            var geometryQualityElement = Root.Q<VisualElement>("geometry-quality");
            var lightingQualityElement = Root.Q<VisualElement>("lighting-quality");
            var shadowsQualityElement = Root.Q<VisualElement>("shadows-quality");
            var particlesQualityElement = Root.Q<VisualElement>("particals-quality");
            var drawingDistanceElement = Root.Q<VisualElement>("drawing-distance");

            _defaultSettingsButton = Root.Q<Button>("defaultSettings");

            // Создание контроллеров
            _maxFpsDropdown = new DropdownController(maxFpsElement.Q<DropdownField>());
            _adaptiveMonitorToggle = new ToggleController(adaptiveMonitorElement.Q("ToogleSlider"));
            _fullscreenModeToggle = new ToggleController(fullscreenModeElement.Q("ToogleSlider"));
            _vSyncToggle = new ToggleController(vSyncElement.Q("ToogleSlider"));
            _gammaSlider = new SliderController(gammaElement.Q("Slider"));

            _qualityLevelCarousel = new CarouselController(qualityLevelElement.Q("Carousel"));
            _textureResolutionCarousel = new CarouselController(textureResolutionElement.Q("Carousel"));
            _geometryQualityCarousel = new CarouselController(geometryQualityElement.Q("Carousel"));
            _lightingQualityCarousel = new CarouselController(lightingQualityElement.Q("Carousel"));
            _shadowsQualityCarousel = new CarouselController(shadowsQualityElement.Q("Carousel"));
            _particlesQualityCarousel = new CarouselController(particlesQualityElement.Q("Carousel"));
            _drawingDistanceCarousel = new CarouselController(drawingDistanceElement.Q("Carousel"));
        }

        protected override void RegisterButtonCallbacks()
        {
            _defaultSettingsButton.clicked += OnDefaultSettingsButtonClicked;
        }

        public void Bind(GraphicsSettingsModel model)
        {
            _model = model;

            // Связывание контроллеров с моделью
            _maxFpsDropdown.Bind(_model.MaxFPS, _model.FpsOptions);
            _adaptiveMonitorToggle.Bind(_model.AdaptiveMonitor);
            _fullscreenModeToggle.Bind(_model.FullscreenMode);
            _vSyncToggle.Bind(_model.VSync);
            _gammaSlider.Bind(_model.Gamma, 0.5f, 2.0f);

            _qualityLevelCarousel.Bind(_model.QualityLevel, _model.QualityOptions);
            _textureResolutionCarousel.Bind(_model.TextureResolution, _model.DetailOptions);
            _geometryQualityCarousel.Bind(_model.GeometryQuality, _model.DetailOptions);
            _lightingQualityCarousel.Bind(_model.LightingQuality, _model.DetailOptions);
            _shadowsQualityCarousel.Bind(_model.ShadowsQuality, _model.DetailOptions);
            _particlesQualityCarousel.Bind(_model.ParticlesQuality, _model.DetailOptions);
            _drawingDistanceCarousel.Bind(_model.DrawingDistance, _model.DetailOptions);
        }

        private void OnDefaultSettingsButtonClicked()
        {
            _model.ResetToDefault();
        }

        public override void Dispose()
        {
            base.Dispose();

            // Очистка ресурсов
            _maxFpsDropdown?.Dispose();
            _adaptiveMonitorToggle?.Dispose();
            _fullscreenModeToggle?.Dispose();
            _vSyncToggle?.Dispose();
            _gammaSlider?.Dispose();

            _qualityLevelCarousel?.Dispose();
            _textureResolutionCarousel?.Dispose();
            _geometryQualityCarousel?.Dispose();
            _lightingQualityCarousel?.Dispose();
            _shadowsQualityCarousel?.Dispose();
            _particlesQualityCarousel?.Dispose();
            _drawingDistanceCarousel?.Dispose();

            if (_defaultSettingsButton != null)
                _defaultSettingsButton.clicked -= OnDefaultSettingsButtonClicked;
        }
    }
}