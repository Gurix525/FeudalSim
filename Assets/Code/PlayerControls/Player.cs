using AI;
using Combat;
using Controls;
using Input;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;

namespace PlayerControls
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerVFX))]
    [RequireComponent(typeof(AimCurve))]
    [RequireComponent(typeof(Stats))]
    public class Player : MonoBehaviour, IDetectable
    {
        #region Fields

        [SerializeField]
        private RightHandItemHook _rightHandItemHook;

        [SerializeField]
        private LeftHandItemHook _leftHandItemHook;

        private Health _health;
        private PlayerMovement _playerMovement;
        private PlayerVFX _playerVFX;
        private AimCurve _aimCurve;
        private Stats _stats;

        #endregion Fields

        #region Properties

        public PlayerMovement PlayerMovement => _playerMovement ??= GetComponent<PlayerMovement>();
        public Stats Stats => _stats ??= GetComponent<Stats>();
        public PlayerVFX VFX => _playerVFX ??= GetComponent<PlayerVFX>();
        public AimCurve AimCurve => _aimCurve ??= GetComponent<AimCurve>();
        public LeftHandItemHook LeftHandItemHook => _leftHandItemHook;
        public RightHandItemHook RightHandItemHook => _rightHandItemHook;

        public static Vector3 Position => Instance.transform.position;

        public static Player Instance { get; set; }

        #endregion Properties



        #region Unity

        private void Awake()
        {
            Instance = this;
            _health = GetComponent<Health>();
            _health.Receiver = this;
        }

        private void OnEnable()
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.AddListener(ActionType.Started, OnRightMouseButton);
            PlayerController.MainLeftClick.AddListener(ActionType.Canceled, OnLeftMouseButtonRelase);
            PlayerController.MainRightClick.AddListener(ActionType.Canceled, OnRightMouseButtonRelase);
        }

        private void Update()
        {
            Cursor.Action.Update();
        }

        private void OnDisable()
        {
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.RemoveListener(ActionType.Started, OnRightMouseButton);
            PlayerController.MainLeftClick.RemoveListener(ActionType.Canceled, OnLeftMouseButtonRelase);
            PlayerController.MainRightClick.RemoveListener(ActionType.Canceled, OnRightMouseButtonRelase);
        }

        #endregion Unity

        #region Private

        private void OnLeftMouseButton(CallbackContext context)
        {
            Cursor.Action.OnLeftMouseButton();
        }

        private void OnLeftMouseButtonRelase(CallbackContext context)
        {
            Cursor.Action.OnLeftMouseButtonRelase();
        }

        private void OnRightMouseButton(CallbackContext context)
        {
            Cursor.Action.OnRightMouseButton();
        }

        private void OnRightMouseButtonRelase(CallbackContext context)
        {
            Cursor.Action.OnRightMouseButtonRelase();
        }

        #endregion Private
    }
}