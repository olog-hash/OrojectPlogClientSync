using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Custom;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    // Контроллер вкладки графики
    public class GraphicsTabController : UIToolkitElementView
    {
        // Контроллеры элементов левой колонки
        private DropdownController _maxFpsDropdown;
        private DropdownController _screenResolutionDropdown;
        private RadioButtonGroupController _vSyncRadioGroup;

        // Контроллеры элементов правой колонки
        private DropdownController _qualityLevelDropdown;
        private DropdownController _renderScaleDropdown;
        private DropdownController _shadowsActiveDropdown;
        private DropdownController _shadowsResolutionDropdown;
        private DropdownController _shadowsDistanceDropdown;
        private DropdownController _antiAliasingDropdown;
        private DropdownController _textureResolutionDropdown;
        private DropdownController _levelOfDetailDropdown;
        private DropdownController _anisotropicFilteringDropdown;

        private Button _defaultSettingsButton;

        private GraphicsSettingsModel _model;

        public GraphicsTabController(VisualElement root) : base(root)
        {
        }

        protected override void SetVisualElements()
        {
            // Получение UI-элементов левой колонки
            var maxFpsElement = Root.Q<VisualElement>("fps-lock");
            var screenResolutionElement = Root.Q<VisualElement>("screen-resolution");
            var vSyncElement = Root.Q<VisualElement>("vsync");

            // Получение UI-элементов правой колонки
            var qualityLevelElement = Root.Q<VisualElement>("quality-level");
            var renderScaleElement = Root.Q<VisualElement>("render-scale");
            var shadowsActiveElement = Root.Q<VisualElement>("shadows-active");
            var shadowsResolutionElement = Root.Q<VisualElement>("shadows-resolution");
            var shadowsDistanceElement = Root.Q<VisualElement>("shadows-distance");
            var antiAliasingElement = Root.Q<VisualElement>("anti-aliasing");
            var textureResolutionElement = Root.Q<VisualElement>("texture-resolution");
            var levelOfDetailElement = Root.Q<VisualElement>("level-of-detail");
            var anisotropicFilteringElement = Root.Q<VisualElement>("anisotropic-filtering");

            _defaultSettingsButton = Root.Q<Button>("defaultSettings");

            // Создание контроллеров для левой колонки
            _maxFpsDropdown = new DropdownController(maxFpsElement.Q<DropdownField>());
            _screenResolutionDropdown = new DropdownController(screenResolutionElement.Q<DropdownField>());
            _vSyncRadioGroup = new RadioButtonGroupController(vSyncElement.Q("RadioButtonGroup"));

            // Создание контроллеров для правой колонки (все выпадающие списки)
            _qualityLevelDropdown = new DropdownController(qualityLevelElement.Q<DropdownField>());
            _renderScaleDropdown = new DropdownController(renderScaleElement.Q<DropdownField>());
            _shadowsActiveDropdown = new DropdownController(shadowsActiveElement.Q<DropdownField>());
            _shadowsResolutionDropdown = new DropdownController(shadowsResolutionElement.Q<DropdownField>());
            _shadowsDistanceDropdown = new DropdownController(shadowsDistanceElement.Q<DropdownField>());
            _antiAliasingDropdown = new DropdownController(antiAliasingElement.Q<DropdownField>());
            _textureResolutionDropdown = new DropdownController(textureResolutionElement.Q<DropdownField>());
            _levelOfDetailDropdown = new DropdownController(levelOfDetailElement.Q<DropdownField>());
            _anisotropicFilteringDropdown = new DropdownController(anisotropicFilteringElement.Q<DropdownField>());
        }

        protected override void RegisterButtonCallbacks()
        {
            _defaultSettingsButton.clicked += OnDefaultSettingsButtonClicked;
        }

        public void Bind(GraphicsSettingsModel model)
        {
            _model = model;

            // Связывание контроллеров левой колонки с моделью
            _maxFpsDropdown.Bind(_model.MaxFPS, _model.FpsOptions);
            _screenResolutionDropdown.Bind(_model.ScreenResolution, _model.ResolutionOptions);
            _vSyncRadioGroup.Bind(_model.VSync, _model.VSyncOptions);

            // Связывание контроллеров правой колонки с моделью
            _qualityLevelDropdown.Bind(_model.QualityLevel, _model.QualityOptions);
            _renderScaleDropdown.Bind(_model.RenderScale, _model.RenderScaleOptions);
            _shadowsActiveDropdown.Bind(_model.ShadowsActive, _model.ShadowsActiveOptions);
            _shadowsResolutionDropdown.Bind(_model.ShadowsResolution, _model.DetailOptions);
            _shadowsDistanceDropdown.Bind(_model.ShadowsDistance, _model.ShadowsDistanceOptions);
            _antiAliasingDropdown.Bind(_model.AntiAliasing, _model.AntiAliasingOptions);
            _textureResolutionDropdown.Bind(_model.TextureResolution, _model.DetailOptions);
            _levelOfDetailDropdown.Bind(_model.LevelOfDetail, _model.LevelOfDetailOptions);
            _anisotropicFilteringDropdown.Bind(_model.AnisotropicFiltering, _model.AnisotropicOptions);
        }

        private void OnDefaultSettingsButtonClicked()
        {
            _model.ResetToDefault();
        }

        public override void Dispose()
        {
            base.Dispose();

            // Очистка всех контроллеров
            _maxFpsDropdown?.Dispose();
            _screenResolutionDropdown?.Dispose();
            _vSyncRadioGroup?.Dispose();

            _qualityLevelDropdown?.Dispose();
            _renderScaleDropdown?.Dispose();
            _shadowsActiveDropdown?.Dispose();
            _shadowsResolutionDropdown?.Dispose();
            _shadowsDistanceDropdown?.Dispose();
            _antiAliasingDropdown?.Dispose();
            _textureResolutionDropdown?.Dispose();
            _levelOfDetailDropdown?.Dispose();
            _anisotropicFilteringDropdown?.Dispose();

            if (_defaultSettingsButton != null)
                _defaultSettingsButton.clicked -= OnDefaultSettingsButtonClicked;
        }
    }
}