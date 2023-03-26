using System;
using Extensions;
using Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class Player : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _movingSpeed = 1F;
        [SerializeField] private float _runningSpeed = 2F;
        [SerializeField] private float _jumpStrength = 1F;

        private CharacterController _controller;
        private float _verticalSpeed = 0F;

        #endregion Fields

        #region Unity

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            PlayerController.MainJump.AddListener(ActionType.Started, Jump);
        }

        private void Update()
        {
            Move();
            UpdateVerticalSpeed();
        }

        private void OnDisable()
        {
            PlayerController.MainJump.ClearStartedEvent();
        }

        #endregion Unity

        #region Private

        private void UpdateVerticalSpeed()
        {
            if (_controller.isGrounded)
                _verticalSpeed = 0F;
            else
                _verticalSpeed -= 9.81F * Time.deltaTime;
        }

        private void Move()
        {
            var direction = PlayerController.MainMove.ReadValue<Vector2>();
            var runMultiplier = PlayerController.MainRun.ReadValue<float>();
            float actualSpeed = runMultiplier.Remap(0F, 1F, _movingSpeed, _runningSpeed);
            _controller.Move(
                new Vector3(direction.x, _verticalSpeed, direction.y)
                * actualSpeed
                * Time.deltaTime);
        }

        private void Jump(CallbackContext context)
        {
            if (_controller.isGrounded)
            {
                _verticalSpeed += _jumpStrength;
            }
        }

        #endregion Private
    }
}