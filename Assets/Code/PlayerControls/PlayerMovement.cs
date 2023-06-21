using System;
using System.Collections;
using Extensions;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float _moveSpeed = 6F;

        [SerializeField]
        private float _acceleration = 10F;

        [SerializeField]
        private float _jumpForce = Physics.gravity.magnitude * 2F;

        [SerializeField]
        private float _gravityForce = Physics.gravity.magnitude;

        private Rigidbody _rigidbody;
        private Animator _animator;

        private bool _isCursorRaycastNull = true;
        private bool _isGrounded;

        private int _groundMask;

        #endregion Fields

        #region Properties

        public bool CanMove => true;

        public bool CanJump => _isGrounded;

        public bool IsGravityEnabled => true;

        public bool CanRotateToCursor => !_isCursorRaycastNull;

        #endregion Properties

        #region Unity

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _groundMask = ~LayerMask.GetMask("Player");
        }

        private void OnEnable()
        {
            PlayerController.MainJump.AddListener(ActionType.Started, Jump);
        }

        private void FixedUpdate()
        {
            CheckConditions();
            if (CanMove)
                Move();
            if (IsGravityEnabled)
                DoGravity();
            if (CanRotateToCursor)
                RotateToCursor();
            SetAnimatorParameters();
        }

        private void OnDisable()
        {
            PlayerController.MainJump.RemoveListener(ActionType.Started, Jump);
        }

        #endregion Unity

        #region Private

        private void CheckConditions()
        {
            _isCursorRaycastNull = Controls.Cursor.ClearRaycastHit == null;
            _isGrounded = Physics.CheckSphere(transform.position, 0.27F, _groundMask);
        }

        private void Move()
        {
            Vector2 direction = PlayerController.MainMove.ReadValue<Vector2>();
            direction *= _moveSpeed;
            float y = _rigidbody.velocity.y;
            _rigidbody.velocity = new(direction.x, y, direction.y);
            //if (correctedDirection != Vector3.zero)
            //    _rigidbody.AddForce(correctedDirection * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //else
            //    _rigidbody.AddForce(-_rigidbody.velocity * _acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange); ;
            //float y = _rigidbody.velocity.y;
            //Vector3 clampedVelocity = Vector3.ClampMagnitude(_rigidbody.velocity, _moveSpeed);
            //_rigidbody.velocity = new(clampedVelocity.x, y, clampedVelocity.z);
        }

        private void DoGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        private void RotateToCursor()
        {
            transform.LookAt(Controls.Cursor.ClearRaycastHit.Value.point);
            transform.rotation = Quaternion.Euler(0F, transform.eulerAngles.y, 0F);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!CanJump)
                return;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        private void SetAnimatorParameters()
        {
            Vector2 velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);
            _animator.SetFloat("Speed", velocity.magnitude);
            _animator.SetBool("IsGrounded", _isGrounded);
        }

        #endregion Private
    }
}