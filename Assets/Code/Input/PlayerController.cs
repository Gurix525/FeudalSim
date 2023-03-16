using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private PlayerInput _playerInput;

    #endregion Fields

    #region Properties

    public static PlayerController Instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }

    public static InputActionMap ActionMapMain { get; private set; }

    public static ModifiableInputAction MainMove { get; set; } = new();

    #endregion Properties

    #region Unity

    private void Awake()
    {
        Instance = this;
        PlayerInput = _playerInput;

        //PlayerInput.SwitchCurrentActionMap("Steering");
        //ActionMapSteering = PlayerInput.currentActionMap;
        //PlayerInput.SwitchCurrentActionMap("Building");
        //ActionMapBuilding = PlayerInput.currentActionMap;
        PlayerInput.SwitchCurrentActionMap("Default");
        ActionMapMain = PlayerInput.currentActionMap;

        MainMove.Action = ActionMapMain.FindAction("Move");
    }

    #endregion Unity
}