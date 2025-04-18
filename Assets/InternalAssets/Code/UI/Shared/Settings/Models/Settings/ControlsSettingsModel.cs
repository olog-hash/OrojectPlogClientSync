using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class ControlsSettingsModel : BaseSettingsModel
    {
        // Реактивные свойства для основных настроек управления
        public ReactiveProperty<int> MouseInverse { get; } = new ReactiveProperty<int>(1); // 0 - да, 1 - нет
        public ReactiveProperty<int> MouseMode { get; } = new ReactiveProperty<int>(0); // 0 - обычный, 1 - альтернативный
        public ReactiveProperty<float> MouseSensitivity { get; } = new ReactiveProperty<float>(50); // от 0 до 100

        // Реактивные свойства для назначения клавиш передвижения
        public ReactiveProperty<KeyCode> MoveForward { get; } = new ReactiveProperty<KeyCode>(KeyCode.W);
        public ReactiveProperty<KeyCode> MoveBackward { get; } = new ReactiveProperty<KeyCode>(KeyCode.S);
        public ReactiveProperty<KeyCode> MoveLeft { get; } = new ReactiveProperty<KeyCode>(KeyCode.A);
        public ReactiveProperty<KeyCode> MoveRight { get; } = new ReactiveProperty<KeyCode>(KeyCode.D);
        public ReactiveProperty<KeyCode> Crouch { get; } = new ReactiveProperty<KeyCode>(KeyCode.LeftControl);
        public ReactiveProperty<KeyCode> Jump { get; } = new ReactiveProperty<KeyCode>(KeyCode.Space);
        public ReactiveProperty<KeyCode> Slow { get; } = new ReactiveProperty<KeyCode>(KeyCode.LeftShift);
        
        // Реактивные свойства для оружия
        public ReactiveProperty<KeyCode> Fire { get; } = new ReactiveProperty<KeyCode>(KeyCode.Mouse0);
        public ReactiveProperty<KeyCode> Aim { get; } = new ReactiveProperty<KeyCode>(KeyCode.Mouse1);
        public ReactiveProperty<KeyCode> Reload { get; } = new ReactiveProperty<KeyCode>(KeyCode.R);
        public ReactiveProperty<KeyCode> PreviousWeapon { get; } = new ReactiveProperty<KeyCode>(KeyCode.Q);
        
        // Реактивные свойства для слотов оружия
        public ReactiveProperty<KeyCode> Weapon1 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha1);
        public ReactiveProperty<KeyCode> Weapon2 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha2);
        public ReactiveProperty<KeyCode> Weapon3 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha3);
        public ReactiveProperty<KeyCode> Weapon4 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha4);
        public ReactiveProperty<KeyCode> Weapon5 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha5);
        public ReactiveProperty<KeyCode> Weapon6 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha6);
        public ReactiveProperty<KeyCode> Weapon7 { get; } = new ReactiveProperty<KeyCode>(KeyCode.Alpha7);
        
        // Реактивные свойства для насмешек и скриншота
        public ReactiveProperty<KeyCode> Taunt1 { get; } = new ReactiveProperty<KeyCode>(KeyCode.F1);
        public ReactiveProperty<KeyCode> Taunt2 { get; } = new ReactiveProperty<KeyCode>(KeyCode.F2);
        public ReactiveProperty<KeyCode> Taunt3 { get; } = new ReactiveProperty<KeyCode>(KeyCode.F3);
        public ReactiveProperty<KeyCode> Screenshot { get; } = new ReactiveProperty<KeyCode>(KeyCode.P);

        // Опции для элементов UI
        public string[] MouseInverseOptions { get; } = { "ДА", "НЕТ" };
        public string[] MouseModeOptions { get; } = { "Обычный", "Альтернат." };

        // Значения по умолчанию
        private readonly int _defaultMouseInverse = 1; // По умолчанию "НЕТ"
        private readonly int _defaultMouseMode = 0; // По умолчанию "Обычный"
        private readonly float _defaultMouseSensitivity = 50f; // По умолчанию среднее значение
        
        // Значения по умолчанию для клавиш (все остальные инициализируются при создании свойств)
        private readonly KeyCode _defaultMoveForward = KeyCode.W;
        private readonly KeyCode _defaultMoveBackward = KeyCode.S;
        private readonly KeyCode _defaultMoveLeft = KeyCode.A;
        private readonly KeyCode _defaultMoveRight = KeyCode.D;
        private readonly KeyCode _defaultCrouch = KeyCode.LeftControl;
        private readonly KeyCode _defaultJump = KeyCode.Space;
        private readonly KeyCode _defaultSlow = KeyCode.LeftShift;
        private readonly KeyCode _defaultFire = KeyCode.Mouse0;
        private readonly KeyCode _defaultAim = KeyCode.Mouse1;
        private readonly KeyCode _defaultReload = KeyCode.R;
        private readonly KeyCode _defaultPreviousWeapon = KeyCode.Q;
        private readonly KeyCode _defaultWeapon1 = KeyCode.Alpha1;
        private readonly KeyCode _defaultWeapon2 = KeyCode.Alpha2;
        private readonly KeyCode _defaultWeapon3 = KeyCode.Alpha3;
        private readonly KeyCode _defaultWeapon4 = KeyCode.Alpha4;
        private readonly KeyCode _defaultWeapon5 = KeyCode.Alpha5;
        private readonly KeyCode _defaultWeapon6 = KeyCode.Alpha6;
        private readonly KeyCode _defaultWeapon7 = KeyCode.Alpha7;
        private readonly KeyCode _defaultTaunt1 = KeyCode.F1;
        private readonly KeyCode _defaultTaunt2 = KeyCode.F2;
        private readonly KeyCode _defaultTaunt3 = KeyCode.F3;
        private readonly KeyCode _defaultScreenshot = KeyCode.P;

        public ControlsSettingsModel()
        {
            // Инициализация
            SetupChangeTracking();
            LoadSettings();
        }

        private void SetupChangeTracking()
        {
            // Подписка на все изменения чтобы отслеживать изменения настроек
            MouseInverse.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MouseMode.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MouseSensitivity.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            
            // Подписка на изменения клавиш
            MoveForward.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MoveBackward.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MoveLeft.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            MoveRight.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Crouch.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Jump.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Slow.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            
            Fire.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Aim.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Reload.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            PreviousWeapon.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            
            Weapon1.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon2.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon3.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon4.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon5.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon6.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Weapon7.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            
            Taunt1.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Taunt2.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Taunt3.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            Screenshot.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
        }

        private void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs
            MouseInverse.Value = PlayerPrefs.GetInt("Controls_MouseInverse", _defaultMouseInverse);
            MouseMode.Value = PlayerPrefs.GetInt("Controls_MouseMode", _defaultMouseMode);
            MouseSensitivity.Value = PlayerPrefs.GetFloat("Controls_MouseSensitivity", _defaultMouseSensitivity);
            
            // Загрузка назначенных клавиш
            MoveForward.Value = (KeyCode)PlayerPrefs.GetInt("Controls_MoveForward", (int)_defaultMoveForward);
            MoveBackward.Value = (KeyCode)PlayerPrefs.GetInt("Controls_MoveBackward", (int)_defaultMoveBackward);
            MoveLeft.Value = (KeyCode)PlayerPrefs.GetInt("Controls_MoveLeft", (int)_defaultMoveLeft);
            MoveRight.Value = (KeyCode)PlayerPrefs.GetInt("Controls_MoveRight", (int)_defaultMoveRight);
            Crouch.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Crouch", (int)_defaultCrouch);
            Jump.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Jump", (int)_defaultJump);
            Slow.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Slow", (int)_defaultSlow);
            
            Fire.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Fire", (int)_defaultFire);
            Aim.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Aim", (int)_defaultAim);
            Reload.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Reload", (int)_defaultReload);
            PreviousWeapon.Value = (KeyCode)PlayerPrefs.GetInt("Controls_PreviousWeapon", (int)_defaultPreviousWeapon);
            
            Weapon1.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon1", (int)_defaultWeapon1);
            Weapon2.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon2", (int)_defaultWeapon2);
            Weapon3.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon3", (int)_defaultWeapon3);
            Weapon4.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon4", (int)_defaultWeapon4);
            Weapon5.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon5", (int)_defaultWeapon5);
            Weapon6.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon6", (int)_defaultWeapon6);
            Weapon7.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Weapon7", (int)_defaultWeapon7);
            
            Taunt1.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Taunt1", (int)_defaultTaunt1);
            Taunt2.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Taunt2", (int)_defaultTaunt2);
            Taunt3.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Taunt3", (int)_defaultTaunt3);
            Screenshot.Value = (KeyCode)PlayerPrefs.GetInt("Controls_Screenshot", (int)_defaultScreenshot);

            SetHasChanges(false);
        }

        public override void ApplySettings()
        {
            // Сохранение настроек в PlayerPrefs
            PlayerPrefs.SetInt("Controls_MouseInverse", MouseInverse.Value);
            PlayerPrefs.SetInt("Controls_MouseMode", MouseMode.Value);
            PlayerPrefs.SetFloat("Controls_MouseSensitivity", MouseSensitivity.Value);
            
            // Сохранение назначенных клавиш
            PlayerPrefs.SetInt("Controls_MoveForward", (int)MoveForward.Value);
            PlayerPrefs.SetInt("Controls_MoveBackward", (int)MoveBackward.Value);
            PlayerPrefs.SetInt("Controls_MoveLeft", (int)MoveLeft.Value);
            PlayerPrefs.SetInt("Controls_MoveRight", (int)MoveRight.Value);
            PlayerPrefs.SetInt("Controls_Crouch", (int)Crouch.Value);
            PlayerPrefs.SetInt("Controls_Jump", (int)Jump.Value);
            PlayerPrefs.SetInt("Controls_Slow", (int)Slow.Value);
            
            PlayerPrefs.SetInt("Controls_Fire", (int)Fire.Value);
            PlayerPrefs.SetInt("Controls_Aim", (int)Aim.Value);
            PlayerPrefs.SetInt("Controls_Reload", (int)Reload.Value);
            PlayerPrefs.SetInt("Controls_PreviousWeapon", (int)PreviousWeapon.Value);
            
            PlayerPrefs.SetInt("Controls_Weapon1", (int)Weapon1.Value);
            PlayerPrefs.SetInt("Controls_Weapon2", (int)Weapon2.Value);
            PlayerPrefs.SetInt("Controls_Weapon3", (int)Weapon3.Value);
            PlayerPrefs.SetInt("Controls_Weapon4", (int)Weapon4.Value);
            PlayerPrefs.SetInt("Controls_Weapon5", (int)Weapon5.Value);
            PlayerPrefs.SetInt("Controls_Weapon6", (int)Weapon6.Value);
            PlayerPrefs.SetInt("Controls_Weapon7", (int)Weapon7.Value);
            
            PlayerPrefs.SetInt("Controls_Taunt1", (int)Taunt1.Value);
            PlayerPrefs.SetInt("Controls_Taunt2", (int)Taunt2.Value);
            PlayerPrefs.SetInt("Controls_Taunt3", (int)Taunt3.Value);
            PlayerPrefs.SetInt("Controls_Screenshot", (int)Screenshot.Value);
            
            // Применение настроек к игре (пустышка, как и просили)
            // TODO: Добавить реальную логику применения настроек управления
            
            SetHasChanges(false);
        }

        public override void ResetToDefault()
        {
            // Сброс настроек к значениям по умолчанию
            MouseInverse.Value = _defaultMouseInverse;
            MouseMode.Value = _defaultMouseMode;
            MouseSensitivity.Value = _defaultMouseSensitivity;
            
            // Сброс назначенных клавиш
            MoveForward.Value = _defaultMoveForward;
            MoveBackward.Value = _defaultMoveBackward;
            MoveLeft.Value = _defaultMoveLeft;
            MoveRight.Value = _defaultMoveRight;
            Crouch.Value = _defaultCrouch;
            Jump.Value = _defaultJump;
            Slow.Value = _defaultSlow;
            
            Fire.Value = _defaultFire;
            Aim.Value = _defaultAim;
            Reload.Value = _defaultReload;
            PreviousWeapon.Value = _defaultPreviousWeapon;
            
            Weapon1.Value = _defaultWeapon1;
            Weapon2.Value = _defaultWeapon2;
            Weapon3.Value = _defaultWeapon3;
            Weapon4.Value = _defaultWeapon4;
            Weapon5.Value = _defaultWeapon5;
            Weapon6.Value = _defaultWeapon6;
            Weapon7.Value = _defaultWeapon7;
            
            Taunt1.Value = _defaultTaunt1;
            Taunt2.Value = _defaultTaunt2;
            Taunt3.Value = _defaultTaunt3;
            Screenshot.Value = _defaultScreenshot;

            SetHasChanges(true);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            // Очистка реактивных свойств
            MouseInverse.Dispose();
            MouseMode.Dispose();
            MouseSensitivity.Dispose();
            
            // Очистка свойств для клавиш
            MoveForward.Dispose();
            MoveBackward.Dispose();
            MoveLeft.Dispose();
            MoveRight.Dispose();
            Crouch.Dispose();
            Jump.Dispose();
            Slow.Dispose();
            
            Fire.Dispose();
            Aim.Dispose();
            Reload.Dispose();
            PreviousWeapon.Dispose();
            
            Weapon1.Dispose();
            Weapon2.Dispose();
            Weapon3.Dispose();
            Weapon4.Dispose();
            Weapon5.Dispose();
            Weapon6.Dispose();
            Weapon7.Dispose();
            
            Taunt1.Dispose();
            Taunt2.Dispose();
            Taunt3.Dispose();
            Screenshot.Dispose();
        }
    }
}