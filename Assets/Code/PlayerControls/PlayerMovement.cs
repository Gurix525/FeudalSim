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
        private float _jumpForce = Physics.gravity.magnitude * 2F;

        [SerializeField]
        private float _gravityForce = Physics.gravity.magnitude;

        [SerializeField]
        private float _sprintMultiplier = 1.25F;

        [SerializeField]
        private float _attackMoveSpeedMultiplier = 1F;

        [SerializeField]
        private float _stringingBowSpeedMultiplier = 0.5F;

        private Rigidbody _rigidbody;
        private Animator _animator;

        private bool _isCursorRaycastNull = true;

        private int _groundMask;
        private int _bowWalkingLayerIndex;

        #endregion Fields

        #region Properties

        public Rigidbody Rigidbody => _rigidbody;

        public float MoveSpeed => _moveSpeed * (IsStringingBow ? _stringingBowSpeedMultiplier : 1F);

        public float SprintSpeed => MoveSpeed * _sprintMultiplier;

        public float AttackMoveSpeedMultiplier => _attackMoveSpeedMultiplier;

        public Vector3 LeftHandIKGoal { get; set; }

        public bool IsPendingAttack { get; set; }

        public bool IsStringingBow { get; set; }

        public int AttackComboNumber { get; set; }

        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }

        #endregion Properties

        #region Conditions

        public bool CanMove => !IsPendingAttack;

        public bool CanJump => IsGrounded && !IsPendingAttack && !IsStringingBow;

        public bool CanSprint => !IsStringingBow && IsGrounded;

        public bool IsGravityEnabled => true;

        public bool CanRotateToCursor => !_isCursorRaycastNull && !IsPendingAttack;

        #endregion Conditions

        #region Public

        public void RotateToCursor()
        {
            transform.LookAt(Controls.Cursor.ClearRaycastHit.Value.point);
            transform.rotation = Quaternion.Euler(0F, transform.eulerAngles.y, 0F);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _groundMask = ~LayerMask.GetMask("Player");
            _bowWalkingLayerIndex = _animator.GetLayerIndex("BowWalking");
        }

        private void OnEnable()
        {
            PlayerController.MainJump.AddListener(ActionType.Started, Jump);
        }

        private void Update()
        {
            SetAnimatorParameters();
        }

        private void FixedUpdate()
        {
            CheckConditions();
            CheckSprint();
            if (CanMove)
                Move();
            if (IsGravityEnabled)
                DoGravity();
            if (CanRotateToCursor)
                RotateToCursor();
        }

        private void OnDisable()
        {
            PlayerController.MainJump.RemoveListener(ActionType.Started, Jump);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            SetAnimatorIK();
        }

        #endregion Unity

        #region Private

        private void CheckConditions()
        {
            _isCursorRaycastNull = Controls.Cursor.ClearRaycastHit == null;
            IsGrounded = Physics.CheckSphere(transform.position, 0.24F, _groundMask);
        }

        private void Move()
        {
            Vector2 direction = PlayerController.MainMove.ReadValue<Vector2>();
            direction *= MoveSpeed * (IsSprinting ? _sprintMultiplier : 1F);
            Vector3 fullDirection = new(direction.x, 0F, direction.y);
            float cameraAngle = Camera.main.transform.eulerAngles.y;
            float y = _rigidbody.velocity.y;
            Vector3 result = new Vector3(fullDirection.x, y, fullDirection.z);
            Quaternion rotation = Quaternion.AngleAxis(cameraAngle, Vector3.up);
            _rigidbody.velocity = rotation * result;
            ImproveRunningSkill(direction);
        }

        private void ImproveRunningSkill(Vector2 direction)
        {
            if (direction == Vector2.zero || !IsSprinting)
                return;
            Player.Instance.Stats.AddSkill("Running", 0.25F * Time.fixedDeltaTime);
        }

        private void DoGravity()
        {
            _rigidbody.AddForce(Vector3.down * _gravityForce * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        private void SetAnimatorParameters()
        {
            Vector2 velocity = new Vector2(
                _rigidbody.velocity.x, _rigidbody.velocity.z);
            Vector2 lookDirection = GetLookDirection();
            float relativeAngle = Vector2
                .SignedAngle(velocity.normalized, lookDirection.normalized)
                .Remap(-180F, 180F, 0F, 360F);
            _animator.SetFloat("Speed", velocity.magnitude);
            _animator.SetFloat("RelativeMoveAngle", relativeAngle);
            _animator.SetFloat("Sprint", IsSprinting ? _sprintMultiplier : 1F);
            _animator.SetBool("IsGrounded", IsGrounded);
            _animator.SetBool("IsAttacking", IsPendingAttack);
            _animator.SetBool("IsStringingBow", IsStringingBow);
            _animator.SetInteger("AttackComboNumber", AttackComboNumber);
            if (IsStringingBow)
            {
                _animator.SetLayerWeight(_bowWalkingLayerIndex, 1F);
            }
            else
            {
                _animator.SetLayerWeight(_bowWalkingLayerIndex, 0F);
            }
        }

        private void SetAnimatorIK()
        {
            if (IsStringingBow)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1F);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKGoal);
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0F);
            }
        }

        private void CheckSprint()
        {
            if (CanSprint)
                if (PlayerController.MainRun.IsPressed())
                {
                    IsSprinting = true;
                    return;
                }
            IsSprinting = false;
        }

        private Vector2 GetLookDirection()
        {
            if (Controls.Cursor.ClearRaycastHit == null)
                return Vector2.zero;
            Vector3 cursorPosition = Controls.Cursor.ClearRaycastHit.Value.point;
            Vector2 transformPosition = new(transform.position.x, transform.position.z);
            Vector2 targetPosition = new(cursorPosition.x, cursorPosition.z);
            return targetPosition - transformPosition;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!CanJump)
                return;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            Player.Instance.Stats.AddSkill("Jumping", 1F);
        }

        #endregion Private
    }
}