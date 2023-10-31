using Controls;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MainCursor _cursor;

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

        private Vector2 _inputVelocity = Vector2.zero;

        private bool _isSprintPressed = false;

        private int _groundMask;
        private int _bowWalkingLayerIndex;

        #endregion Fields

        #region Properties

        public Rigidbody Rigidbody => _rigidbody;

        public float MoveSpeed =>
            _moveSpeed * (IsStringingBow ? _stringingBowSpeedMultiplier : 1F);

        public float SprintSpeed =>
            MoveSpeed * (_sprintMultiplier + (_sprintMultiplier - 1F)
            * Player.Current.Stats.GetSkill("Running").Modifier);

        public float AttackMoveSpeedMultiplier => _attackMoveSpeedMultiplier;

        public float JumpForce => _jumpForce
            + 4F * Player.Current.Stats.GetSkill("Jumping").Modifier;

        public Vector3 LeftHandIKGoal { get; set; }

        public bool IsPendingAttack { get; set; }

        public bool IsStringingBow { get; set; }

        public int AttackComboNumber { get; set; }

        public bool IsGrounded { get; private set; }
        public bool IsSprinting { get; private set; }

        public static PlayerMovement Current { get; private set; }

        #endregion Properties

        #region Conditions

        public bool HasStamina => Player.Current.Stats.CurrentStamina > 0F;

        public bool CanAttack => !IsPendingAttack
            && !IsStringingBow
            && IsGrounded;

        public bool CanMove => !IsPendingAttack;

        public bool CanJump => IsGrounded && !IsPendingAttack && !IsStringingBow && HasStamina;

        public bool CanSprint => !IsStringingBow && IsGrounded && HasStamina;

        public bool IsGravityEnabled => true;

        public bool CanRotateToCursor => !IsPendingAttack;

        #endregion Conditions

        #region Public

        public void RotateToCursor()
        {
            transform.LookAt(_cursor.WorldRaycastHit.point);
            transform.rotation = Quaternion.Euler(0F, transform.eulerAngles.y, 0F);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Current = this;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _groundMask = ~LayerMask.GetMask("Player");
            _bowWalkingLayerIndex = _animator.GetLayerIndex("BowWalking");
        }

        private void OnEnable()
        {
            Player.Current.Stats.StaminaDepleted.AddListener(() => _isSprintPressed = false);
        }

        private void OnDisable()
        {
            Player.Current.Stats.StaminaDepleted.RemoveAllListeners();
        }

        private void Update()
        {
            SetAnimatorParameters();
        }

        private void FixedUpdate()
        {
            CheckConditions();
            CheckSprint();
            Move();
            if (IsGravityEnabled)
                DoGravity();
            if (CanRotateToCursor)
                RotateToCursor();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            SetAnimatorIK();
        }

        #endregion Unity

        #region Private

        private void CheckConditions()
        {
            IsGrounded = Physics.CheckSphere(transform.position, 0.24F, _groundMask);
        }

        private void Move()
        {
            if (!CanMove)
                return;
            Vector2 direction = _inputVelocity;
            direction *= MoveSpeed * (IsSprinting ? _sprintMultiplier : 1F);
            Vector3 fullDirection = new(direction.x, 0F, direction.y);
            float cameraAngle = Camera.main.transform.eulerAngles.y;
            float y = _rigidbody.velocity.y;
            Vector3 result = new Vector3(fullDirection.x, y, fullDirection.z);
            Quaternion rotation = Quaternion.AngleAxis(cameraAngle, Vector3.up);
            _rigidbody.velocity = rotation * result;
            if (direction != Vector2.zero && IsSprinting)
            {
                ImproveRunningSkill(direction);
                SubtractStamina(Time.fixedDeltaTime * 20F);
            }
        }

        private void OnMove(InputValue value)
        {
            _inputVelocity = value.Get<Vector2>();
        }

        private void OnJump(InputValue value)
        {
            if (!CanJump)
                return;
            _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            Player.Current.Stats.AddSkill("Jumping", 1F);
            SubtractStamina(15F);
        }

        private void OnSprint(InputValue value)
        {
            _isSprintPressed = value.isPressed;
        }

        private void ImproveRunningSkill(Vector2 direction)
        {
            Player.Current.Stats.AddSkill("Running", 0.25F * Time.fixedDeltaTime);
        }

        private void SubtractStamina(float stamina)
        {
            Player.Current.Stats.CurrentStamina -= stamina;
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
                if (_isSprintPressed)
                {
                    IsSprinting = true;
                    return;
                }
            IsSprinting = false;
        }

        private Vector2 GetLookDirection()
        {
            Vector3 cursorPosition = _cursor.WorldRaycastHit.point;
            Vector2 transformPosition = new(transform.position.x, transform.position.z);
            Vector2 targetPosition = new(cursorPosition.x, cursorPosition.z);
            return targetPosition - transformPosition;
        }

        #endregion Private
    }
}