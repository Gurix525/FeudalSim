using System;
using AI;
using Combat;
using Extensions;
using Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour, IDetectable
    {
        #region Fields

        private Animator _animator;
        private Health _health;

        #endregion Fields

        #region Public

        public static void SetAnimatorBool(string parameterName, bool value)
        {
            Instance._animator.SetBool(parameterName, true);
        }

        #endregion Public

        #region Properties

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
        }

        private void OnEnable()
        {
            PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.AddListener(ActionType.Started, OnRightMouseButton);
        }

        private void Update()
        {
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

        #endregion Private
    }
}