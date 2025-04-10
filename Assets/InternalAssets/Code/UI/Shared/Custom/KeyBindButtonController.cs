using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    /// <summary>
    /// Контроллер для кнопки привязки клавиш
    /// </summary>
    public class KeyBindButtonController : UIToolkitElementView
    {
        private Button _button;
        private ReactiveProperty<KeyCode> _keyBinding;
        private bool _isListening = false;
        private IPanel _panel;

        // Текст, отображаемый при ожидании нажатия клавиши
        private const string LISTENING_TEXT = "Нажмите кнопку";

        // CSS класс для состояния ожидания
        private const string LISTENING_CLASS = "key-bind-button--listening";

        public KeyBindButtonController(VisualElement root) : base(root)
        {
        }

        protected override void SetVisualElements()
        {
            _button = Root as Button;
            if (_button == null)
            {
                Debug.LogError("Root элемент должен быть Button");
            }
        }

        protected override void RegisterButtonCallbacks()
        {
            _button.clicked += OnBindButtonClicked;
        }

        /// <summary>
        /// Привязывает контроллер к реактивному свойству для хранения клавиши
        /// </summary>
        public void Bind(ReactiveProperty<KeyCode> keyBinding)
        {
            _keyBinding = keyBinding;

            // Отображаем текущую привязанную клавишу
            UpdateKeyText(_keyBinding.Value);

            // Подписываемся на изменения в модели
            _keyBinding
                .Subscribe(newKey =>
                {
                    // Обновляем текст только если мы не в режиме прослушивания
                    if (!_isListening)
                    {
                        UpdateKeyText(newKey);
                    }
                })
                .AddTo(_disposables);

            // Добавляем обработчики событий
            _panel = _button.panel;
            if (_panel != null)
            {
                var visualTree = _panel.visualTree;

                // Подписываемся на события клавиатуры
                visualTree.RegisterCallback<KeyDownEvent>(OnGlobalKeyDown, TrickleDown.TrickleDown);

                // Подписываемся на события мыши
                visualTree.RegisterCallback<MouseDownEvent>(OnGlobalMouseDown, TrickleDown.TrickleDown);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку привязки
        /// </summary>
        private void OnBindButtonClicked()
        {
            if (!_isListening)
            {
                StartListening();
            }
            else
            {
                StopListening();
            }
        }

        /// <summary>
        /// Глобальный обработчик нажатия клавиш
        /// </summary>
        private void OnGlobalKeyDown(KeyDownEvent evt)
        {
            if (_isListening && evt.keyCode != KeyCode.None)
            {
                // Если нажата Escape - отменяем изменение
                if (evt.keyCode == KeyCode.Escape)
                {
                    StopListening();
                }
                else
                {
                    // Записываем новую привязанную клавишу и прекращаем прослушивание
                    _keyBinding.Value = evt.keyCode;
                    StopListening();
                }

                // Предотвращаем дальнейшую обработку этой клавиши
                evt.StopPropagation();
                evt.PreventDefault();
            }
        }

        /// <summary>
        /// Глобальный обработчик нажатия кнопок мыши
        /// </summary>
        private void OnGlobalMouseDown(MouseDownEvent evt)
        {
            if (_isListening)
            {
                KeyCode mouseKey = KeyCode.None;

                // Определяем, какая кнопка мыши была нажата
                switch (evt.button)
                {
                    case 0:
                        // Не привязываем ЛКМ если это было первое нажатие для входа в режим прослушивания
                        if (!_button.ContainsPoint(evt.localMousePosition))
                        {
                            mouseKey = KeyCode.Mouse0;
                        }

                        break;
                    case 1:
                        mouseKey = KeyCode.Mouse1;
                        break;
                    case 2:
                        mouseKey = KeyCode.Mouse2;
                        break;
                    case 3:
                        mouseKey = KeyCode.Mouse3;
                        break;
                    case 4:
                        mouseKey = KeyCode.Mouse4;
                        break;
                    case 5:
                        mouseKey = KeyCode.Mouse5;
                        break;
                    case 6:
                        mouseKey = KeyCode.Mouse6;
                        break;
                }

                if (mouseKey != KeyCode.None)
                {
                    _keyBinding.Value = mouseKey;
                    StopListening();

                    evt.StopPropagation();
                    evt.PreventDefault();
                }
            }
        }

        /// <summary>
        /// Запускает режим прослушивания для назначения клавиши
        /// </summary>
        private void StartListening()
        {
            _isListening = true;
            _button.text = LISTENING_TEXT;
            _button.AddToClassList(LISTENING_CLASS);
            _button.Focus();
        }

        /// <summary>
        /// Останавливает режим прослушивания
        /// </summary>
        private void StopListening()
        {
            _isListening = false;
            UpdateKeyText(_keyBinding.Value);
            _button.RemoveFromClassList(LISTENING_CLASS);
        }

        /// <summary>
        /// Обновляет текст для отображения назначенной клавиши
        /// </summary>
        private void UpdateKeyText(KeyCode keyCode)
        {
            _button.text = keyCode == KeyCode.None ? "Не назначено" : GetKeyDisplayName(keyCode);
        }

        /// <summary>
        /// Получает отображаемое название клавиши
        /// </summary>
        private string GetKeyDisplayName(KeyCode keyCode)
        {
            // Для некоторых специальных клавиш делаем более понятные названия
            switch (keyCode)
            {
                case KeyCode.Return: return "Enter";
                case KeyCode.Escape: return "Esc";
                case KeyCode.Mouse0: return "ЛКМ";
                case KeyCode.Mouse1: return "ПКМ";
                case KeyCode.Mouse2: return "СКМ";
                case KeyCode.Mouse3: return "Мышь 4";
                case KeyCode.Mouse4: return "Мышь 5";
                case KeyCode.Mouse5: return "Мышь 6";
                case KeyCode.Mouse6: return "Мышь 7";
                case KeyCode.LeftAlt: return "Left Alt";
                case KeyCode.RightAlt: return "Right Alt";
                case KeyCode.LeftControl: return "Left Ctrl";
                case KeyCode.RightControl: return "Right Ctrl";
                case KeyCode.LeftShift: return "Left Shift";
                case KeyCode.RightShift: return "Right Shift";
                default: return keyCode.ToString();
            }
        }

        public override void Dispose()
        {
            // Останавливаем прослушивание при уничтожении
            if (_isListening)
            {
                StopListening();
            }

            // Отписываемся от глобальных событий
            if (_panel != null)
            {
                var visualTree = _panel.visualTree;
                visualTree.UnregisterCallback<KeyDownEvent>(OnGlobalKeyDown, TrickleDown.TrickleDown);
                visualTree.UnregisterCallback<MouseDownEvent>(OnGlobalMouseDown, TrickleDown.TrickleDown);
                _panel = null;
            }

            base.Dispose();
        }
    }
}