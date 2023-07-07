using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private PlayerInput _playerInput;

        private static bool _isInitialized = false;

        #endregion Fields

        #region Properties

        public static PlayerInput PlayerInput { get; private set; }

        public static InputActionMap Main { get; private set; }
        public static InputActionMap QuickMenu { get; private set; }

        // Main
        public static ModifiableInputAction MainMove { get; } = new();

        public static ModifiableInputAction MainRun { get; } = new();
        public static ModifiableInputAction MainJump { get; } = new();
        public static ModifiableInputAction MainLeftClick { get; } = new();
        public static ModifiableInputAction MainPoint { get; } = new();
        public static ModifiableInputAction MainChange { get; } = new();
        public static ModifiableInputAction MainRightClick { get; } = new();
        public static ModifiableInputAction MainEscape { get; } = new();
        public static ModifiableInputAction MainMouseDelta { get; } = new();
        public static ModifiableInputAction MainQuickMenu { get; } = new();
        public static ModifiableInputAction MainControl { get; } = new();
        public static ModifiableInputAction MainAlt { get; } = new();
        public static ModifiableInputAction MainTab { get; } = new();
        public static ModifiableInputAction MainHotbar1 { get; } = new();
        public static ModifiableInputAction MainHotbar2 { get; } = new();
        public static ModifiableInputAction MainHotbar3 { get; } = new();
        public static ModifiableInputAction MainHotbar4 { get; } = new();
        public static ModifiableInputAction MainHotbar5 { get; } = new();
        public static ModifiableInputAction MainHotbar6 { get; } = new();
        public static ModifiableInputAction MainHotbar7 { get; } = new();
        public static ModifiableInputAction MainHotbar8 { get; } = new();
        public static ModifiableInputAction MainSave { get; } = new();
        public static ModifiableInputAction MainLoad { get; } = new();
        public static ModifiableInputAction MainScroll { get; } = new();
        public static ModifiableInputAction MainMiddleMouseButton { get; } = new();
        public static ModifiableInputAction MainSkills { get; } = new();

        // QuickMenu
        public static ModifiableInputAction QuickMenuQuickMenu { get; } = new();

        public static ModifiableInputAction QuickMenuMouseDelta { get; } = new();

        #endregion Properties

        #region Public

        public static void SwitchCurrentActionMap(string name)
        {
            PlayerInput.SwitchCurrentActionMap(name);
        }

        public static void Initialize()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;

            PlayerInput = FindObjectOfType<PlayerController>()._playerInput;

            // ActionMaps initialization
            PlayerInput.SwitchCurrentActionMap("QuickMenu");
            QuickMenu = PlayerInput.currentActionMap;
            PlayerInput.SwitchCurrentActionMap("Main");
            Main = PlayerInput.currentActionMap;
            QuickMenu.Enable();

            // Main
            MainMove.Action = Main.FindAction("Move");
            MainRun.Action = Main.FindAction("Run");
            MainJump.Action = Main.FindAction("Jump");
            MainLeftClick.Action = Main.FindAction("LeftClick");
            MainPoint.Action = Main.FindAction("Point");
            MainChange.Action = Main.FindAction("Change");
            MainRightClick.Action = Main.FindAction("RightClick");
            MainEscape.Action = Main.FindAction("Escape");
            MainMouseDelta.Action = Main.FindAction("MouseDelta");
            MainQuickMenu.Action = Main.FindAction("QuickMenu");
            MainControl.Action = Main.FindAction("Control");
            MainTab.Action = Main.FindAction("Tab");
            MainAlt.Action = Main.FindAction("Alt");
            MainHotbar1.Action = Main.FindAction("Hotbar1");
            MainHotbar2.Action = Main.FindAction("Hotbar2");
            MainHotbar3.Action = Main.FindAction("Hotbar3");
            MainHotbar4.Action = Main.FindAction("Hotbar4");
            MainHotbar5.Action = Main.FindAction("Hotbar5");
            MainHotbar6.Action = Main.FindAction("Hotbar6");
            MainHotbar7.Action = Main.FindAction("Hotbar7");
            MainHotbar8.Action = Main.FindAction("Hotbar8");
            MainSave.Action = Main.FindAction("Save");
            MainLoad.Action = Main.FindAction("Load");
            MainScroll.Action = Main.FindAction("Scroll");
            MainMiddleMouseButton.Action = Main.FindAction("MiddleMouseButton");
            MainSkills.Action = Main.FindAction("Skills");

            // QuickMenu
            QuickMenuQuickMenu.Action = QuickMenu.FindAction("QuickMenu");
            QuickMenuMouseDelta.Action = QuickMenu.FindAction("MouseDelta");
        }

        #endregion Public
    }
}