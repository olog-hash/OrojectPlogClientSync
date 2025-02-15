using System.Collections.Generic;
using UnityEngine;

namespace ProjectOlog.Code.Input.Controls
{
    public class InputControls
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        private const string MouseX = "Mouse X";
        private const string MouseY = "Mouse Y";

        // Параметры
        public static bool IsFirstPersonModeEnabled = true;
        public static bool IsMouseControlEnabled = true;
        public static bool IsKeyControlEnabled = true;
        public static Dictionary<KeyType, KeyCode> KeyDictionary = new Dictionary<KeyType, KeyCode>();

        public static void Reset()
        {
            IsFirstPersonModeEnabled = true;
            IsMouseControlEnabled = true;
            IsKeyControlEnabled = true;

            KeyDictionary = new Dictionary<KeyType, KeyCode>()
            {
                { KeyType.None, KeyCode.None },
                { KeyType.Fire, KeyCode.Mouse0 },
                { KeyType.Middle, KeyCode.Mouse2 },
                { KeyType.AltFire, KeyCode.Mouse1 },
                { KeyType.Move_Forward, KeyCode.W },
                { KeyType.Move_Backward, KeyCode.S },
                { KeyType.Move_Left, KeyCode.A },
                { KeyType.Move_Right, KeyCode.D },
                { KeyType.Move_Jump, KeyCode.Space },
                { KeyType.Move_Crouch, KeyCode.C },
                { KeyType.Move_Shift, KeyCode.LeftShift },
                { KeyType.Move_Down, KeyCode.LeftControl },
                { KeyType.NoClip, KeyCode.Q },
                { KeyType.Ability_1, KeyCode.T },
                { KeyType.Ability_2, KeyCode.None },
                { KeyType.Ability_3, KeyCode.None },
                { KeyType.Ability_4, KeyCode.None },
                { KeyType.Alpha_1, KeyCode.Alpha1 },
                { KeyType.Alpha_2, KeyCode.Alpha2 },
                { KeyType.Alpha_3, KeyCode.Alpha3 },
                { KeyType.Use, KeyCode.F },
            };
        }

        public static Vector2 GetMoveAxis()
        {
            Vector2 moveInput = Vector2.zero;

            moveInput.y += InputControls.GetKey(KeyType.Move_Forward) ? 1f : 0f;
            moveInput.y += InputControls.GetKey(KeyType.Move_Backward) ? -1f : 0f;
            moveInput.x += InputControls.GetKey(KeyType.Move_Right) ? 1f : 0f;
            moveInput.x += InputControls.GetKey(KeyType.Move_Left) ? -1f : 0f;

            return moveInput;
        }

        #region Expanded

        public static void SetFirstPersonMode() => IsFirstPersonModeEnabled = true;
        public static void SetThirdPersonMode() => IsFirstPersonModeEnabled = false;
        
        public static Vector2 GetFirstPersonLookAxis()
        {
            if (!IsFirstPersonModeEnabled) return Vector2.zero;

            return GetLookAxis();
        }

        public static Vector2 GetFirstPersonLookAxisRaw()
        {
            if (!IsFirstPersonModeEnabled) return Vector2.zero;

            return GetLookAxisRaw();
        }
        
        public static Vector2 GetThirdPersonLookAxis()
        {
            if (IsFirstPersonModeEnabled) return Vector2.zero;

            return GetLookAxis();
        }

        public static Vector2 GetThirdPersonLookAxisRaw()
        {
            if (IsFirstPersonModeEnabled) return Vector2.zero;

            return GetLookAxisRaw();
        }

        #endregion
        
        #region Basics
        public static Vector2 GetLookAxis()
        {
            if (!IsMouseControlEnabled) return Vector2.zero;

            return new Vector2(UnityEngine.Input.GetAxis(MouseX), UnityEngine.Input.GetAxis(MouseY));
        }

        public static Vector2 GetLookAxisRaw()
        {
            if (!IsMouseControlEnabled) return Vector2.zero;

            return new Vector2(UnityEngine.Input.GetAxisRaw(MouseX), UnityEngine.Input.GetAxisRaw(MouseY));
        }

        public static float GetAxis(string name)
        {
            if (!IsMouseControlEnabled) return 0f;

            return UnityEngine.Input.GetAxis(name);
        }

        public static float GetAxisRaw(string name)
        {
            if (!IsMouseControlEnabled) return 0f;

            return UnityEngine.Input.GetAxisRaw(name);
        }

        public static bool GetMouseButton(int id)
        {
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetMouseButton(id);
        }

        public static bool GetMouseButtonUp(int id)
        {
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetMouseButtonUp(id);
        }

        public static bool GetMouseButtonDown(int id)
        {
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetMouseButtonDown(id);
        }

        public static bool GetKey(KeyType type)
        {
            if (!KeyDictionary.ContainsKey(type)) return false;
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetKey(KeyDictionary[type]);
        }

        public static bool GetKeyDown(KeyType type)
        {
            if (!KeyDictionary.ContainsKey(type)) return false;
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetKeyDown(KeyDictionary[type]);
        }

        public static bool GetKeyUp(KeyType type)
        {
            if (!KeyDictionary.ContainsKey(type)) return false;
            if (!IsKeyControlEnabled) return false;

            return UnityEngine.Input.GetKeyUp(KeyDictionary[type]);
        }
        #endregion
    }

    public enum KeyType
    {
        None,
        Fire,
        Middle,
        AltFire,
        Move_Forward,
        Move_Backward,
        Move_Left,
        Move_Right,
        Move_Jump,
        Move_Crouch,
        Move_Shift,
        Move_Down,
        NoClip,
        Ability_1, 
        Ability_2, 
        Ability_3, 
        Ability_4,
        Use,
        Alpha_1,
        Alpha_2,
        Alpha_3,
        // Камера
    }
}
