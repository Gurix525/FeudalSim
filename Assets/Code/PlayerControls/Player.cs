using AI;
using Combat;
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
    public class Player : MonoBehaviour, IDetectable
    {
        #region Fields

        private Health _health;
        private ThirdPersonController _thirdPersonController;
        private PlayerMovement _playerMovement;

        #endregion Fields

        #region Properties

        public PlayerMovement PlayerMovement => _playerMovement ??= GetComponent<PlayerMovement>();

        public static UnityEvent<bool> PendingAttack { get; } = new();

        public static UnityEvent AttackChanged { get; } = new();

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

        private void OnPendingAttack(bool state)
        {
            PendingAttack.Invoke(state);
        }

        private void OnAttackChanged()
        {
            AttackChanged.Invoke();
        }

        #endregion Private
    }
}