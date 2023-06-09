using AI;
using Combat;
using Input;
using StateMachineBehaviours;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour, IDetectable
    {
        #region Fields

        private Health _health;
        private Animator _animator;
        private AttackStartStop[] _attackStartStops;

        #endregion Fields

        #region Properties

        public static UnityEvent<bool> PendingAttack { get; } = new();

        public static Vector3 Position => Instance.transform.position;

        public static Player Instance { get; set; }

        #endregion Properties

        #region Unity

        private void Awake()
        {
            Instance = this;
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
            _health.Receiver = this;
            _animator = GetComponent<Animator>();
            _attackStartStops = _animator.GetBehaviours<AttackStartStop>();
            foreach (var attackStartStop in _attackStartStops)
                attackStartStop.PendingAttack.AddListener(OnPendingAttack);
        }

        private void OnEnable()
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.AddListener(ActionType.Started, OnRightMouseButton);
        }

        private void Update()
        {
            if (_attackStartStops[0] == null)
            {
                _attackStartStops = _animator.GetBehaviours<AttackStartStop>();
                foreach (var attackStartStop in _attackStartStops)
                    attackStartStop.PendingAttack.AddListener(OnPendingAttack);
            }
            Cursor.Action.Update();
        }

        private void OnDisable()
        {
            PlayerController.MainLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.RemoveListener(ActionType.Started, OnRightMouseButton);
        }

        #endregion Unity

        #region Private

        private void OnLeftMouseButton(CallbackContext context)
        {
            Cursor.Action.OnLeftMouseButton();
        }

        private void OnRightMouseButton(CallbackContext context)
        {
            Cursor.Action.OnRightMouseButton();
        }

        private void OnPendingAttack(bool state)
        {
            PendingAttack.Invoke(state);
        }

        #endregion Private
    }
}