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

        public static ModifiableInputAction MainMove { get; set; } = new();
        public static ModifiableInputAction MainRun { get; set; } = new();
        public static ModifiableInputAction MainJump { get; set; } = new();
        public static ModifiableInputAction MainUse { get; set; } = new();
        public static ModifiableInputAction MainPoint { get; set; } = new();
        public static ModifiableInputAction MainChange { get; set; } = new();
        public static ModifiableInputAction MainRightClick { get; set; } = new();

        #endregion Properties

        #region Public

        public static void Initialize()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;

            PlayerInput = FindObjectOfType<PlayerController>()._playerInput;

            //PlayerInput.SwitchCurrentActionMap("Steering");
            //ActionMapSteering = PlayerInput.currentActionMap;
            //PlayerInput.SwitchCurrentActionMap("Building");
            //ActionMapBuilding = PlayerInput.currentActionMap;
            PlayerInput.SwitchCurrentActionMap("Main");
            Main = PlayerInput.currentActionMap;

            MainMove.Action = Main.FindAction("Move");
            MainRun.Action = Main.FindAction("Run");
            MainJump.Action = Main.FindAction("Jump");
            MainUse.Action = Main.FindAction("Use");
            MainPoint.Action = Main.FindAction("Point");
            MainChange.Action = Main.FindAction("Change");
            MainRightClick.Action = Main.FindAction("RightClick");
        }

        #endregion Public
    }
}