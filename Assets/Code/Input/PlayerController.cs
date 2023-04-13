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
        public static ModifiableInputAction MainMove { get; set; } = new();

        public static ModifiableInputAction MainRun { get; set; } = new();
        public static ModifiableInputAction MainJump { get; set; } = new();
        public static ModifiableInputAction MainUse { get; set; } = new();
        public static ModifiableInputAction MainPoint { get; set; } = new();
        public static ModifiableInputAction MainChange { get; set; } = new();
        public static ModifiableInputAction MainRightClick { get; set; } = new();
        public static ModifiableInputAction MainEscape { get; set; } = new();
        public static ModifiableInputAction MainMouseDelta { get; set; } = new();
        public static ModifiableInputAction MainQuickMenu { get; set; } = new();

        // QuickMenu
        public static ModifiableInputAction QuickMenuQuickMenu { get; set; } = new();

        public static ModifiableInputAction QuickMenuMouseDelta { get; set; } = new();

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
            MainUse.Action = Main.FindAction("Use");
            MainPoint.Action = Main.FindAction("Point");
            MainChange.Action = Main.FindAction("Change");
            MainRightClick.Action = Main.FindAction("RightClick");
            MainEscape.Action = Main.FindAction("Escape");
            MainMouseDelta.Action = Main.FindAction("MouseDelta");
            MainQuickMenu.Action = Main.FindAction("QuickMenu");

            // QuickMenu
            QuickMenuQuickMenu.Action = QuickMenu.FindAction("QuickMenu");
            QuickMenuMouseDelta.Action = QuickMenu.FindAction("MouseDelta");
        }

        #endregion Public
    }
}