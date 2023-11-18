using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Extensions;
using TaskManager;
using UnityEngine;
using UnityEngine.Events;
using VFX;
using WorldUI;

namespace AI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Agent))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Stats))]
    public abstract class Entity : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Vector3 _healthBarOffset = Vector3.up;
        [SerializeField] private Attack[] _attacks;

        protected Agent _agent;

        private Health _health;
        private Animator _animator;
        private Stats _stats;
        private System.Random _random = new();
        private float _idleType = 0F;
        private float _timeSinceIdleTypeRandomization = 0F;
        private MoveSpeedType _moveSpeedType;
        private bool _isKnockbackActive;
        private bool _isBeingDestroyed;
        private Task _knockbackTask;

        private Dictionary<MoveSpeedType, MoveSpeed> _moveSpeeds = new()
        {
            { MoveSpeedType.Walk, new(2F, 2F) },
            { MoveSpeedType.Trot, new(4F, 4F) },
            { MoveSpeedType.Chase, new(6F, 6F) },
            { MoveSpeedType.RunAway, new(8F, 8F) }
        };

        private static AIHealthBar _healthBarPrefab;

        #endregion Fields

        #region Properties

        public UnityEvent<Hitbox> DealedHit { get; } = new();
        public Stats Stats => _stats;
        public IReadOnlyCollection<Attack> Attacks => _attacks;

        public MoveSpeedType MoveSpeed
        {
            get => _moveSpeedType;
            set
            {
                _moveSpeedType = value;
                SetSpeed(value);
            }
        }

        #endregion Properties

        #region Public

        /// <summary>
        /// -1 oznacza wszystkie ataki przypisane do tego zwierzęcia
        /// </summary>
        public void SetAttackActive(bool state, int index = -1)
        {
            if (index == -1)
            {
                foreach (Attack attack in Attacks)
                    attack.gameObject.SetActive(state);
                if (state)
                    foreach (Attack attack in Attacks)
                        attack.SetNextID();
                return;
            }
            if (index >= _attacks.Length)
                throw new IndexOutOfRangeException("Nie ma ataku o takim indeksie.");
            _attacks[index].gameObject.SetActive(state);
            _attacks[index].SetNextID();
        }

        public void SetAttackDamage(float damage, int index = -1)
        {
            if (index == -1)
            {
                foreach (Attack attack in Attacks)
                    attack.Damage = damage;
                return;
            }
            if (index >= _attacks.Length)
                throw new IndexOutOfRangeException("Nie ma ataku o takim indeksie.");
            _attacks[index].Damage = damage;
        }

        public void SetAttackTarget(Component target, int index = -1)
        {
            if (index == -1)
            {
                foreach (Attack attack in Attacks)
                    attack.Target = target;
                return;
            }
            if (index >= _attacks.Length)
                throw new IndexOutOfRangeException("Nie ma ataku o takim indeksie.");
            _attacks[index].Target = target;
        }

        #endregion Public

        #region Unity

        protected void Awake()
        {
            _agent = GetComponent<Agent>();
            _animator = GetComponent<Animator>();
            _stats = GetComponent<Stats>();
            SetSpeed(MoveSpeedType.Walk);
            InitializeHealth();
            InitializeHealthBar();
            InitializeAttacks();
        }

        private void Update()
        {
            SetAnimatorParameters();
        }

        protected virtual void FixedUpdate()
        {
            RandomizeIdleType();
        }

        #endregion Unity

        #region Protected

        protected virtual void SetAnimatorParameters()
        {
            _animator.SetFloat("MoveSpeed", _agent.Velocity.magnitude);
            _animator.SetFloat("IdleType", Mathf.Lerp(_animator.GetFloat("IdleType"), _idleType, 0.05F));
        }

        #endregion Protected

        #region Private

        private void SetSpeed(MoveSpeedType value)
        {
            _agent.Speed = _moveSpeeds[value].Speed;
            _agent.Acceleration = _moveSpeeds[value].Acceleration;
        }

        private void OnGotHit(Attack attack)
        {
            if (_isBeingDestroyed)
                return;
            _knockbackTask?.Stop();
            _stats.CurrentHP -= attack.Damage;
            if (_stats.CurrentHP <= 0F)
            {
                Effect.Spawn("DeathBloodCloud", transform.position + Vector3.up);
                Effect.Spawn("DeathBloodSplatter", transform.position + Vector3.up);
                Effect.Spawn("DeathHit", transform.position + Vector3.up);
                DestroySafely();
                return;
            }
            _knockbackTask = new(KnockBack(attack));
        }

        private void InitializeHealth()
        {
            _health = GetComponent<Health>();
            _health.Receiver = this;
            _health.GotHit.AddListener(OnGotHit);
        }

        private void InitializeHealthBar()
        {
            var healthBar = Instantiate(
                _healthBarPrefab ??=
                Resources.Load<AIHealthBar>("Prefabs/WorldUI/AIHealthBar"),
                transform);
            healthBar.Initialize(_stats, _healthBarOffset);
        }

        private void InitializeAttacks()
        {
            foreach (Attack attack in Attacks)
            {
                attack.Sender = this;
                attack.Damage = 4F;
                attack.DealedHit.AddListener((hitbox, contact) => DealedHit.Invoke(hitbox));
            }
            SetAttackActive(false);
        }

        private void RandomizeIdleType()
        {
            if (_timeSinceIdleTypeRandomization > 5F)
            {
                _timeSinceIdleTypeRandomization = 0F;
                _idleType = _random.NextFloat();
            }
            _timeSinceIdleTypeRandomization += Time.fixedDeltaTime;
        }

        private IEnumerator KnockBack(Attack attack)
        {
            _isKnockbackActive = true;
            float elapsedTime = 0F;
            float blockedTime = 0.25F;
            Vector3 direction = (transform.position - attack.transform.position).normalized * 10F;
            while (elapsedTime < blockedTime)
            {
                elapsedTime += Time.fixedDeltaTime;
                _agent.Move(direction * Time.fixedDeltaTime * (blockedTime - elapsedTime) / blockedTime);
                yield return new WaitForFixedUpdate();
            }
            _isKnockbackActive = false;
        }

        private void DestroySafely()
        {
            _knockbackTask?.Stop();
            _isBeingDestroyed = true;
            _agent.Disable();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        #endregion Private
    }
}