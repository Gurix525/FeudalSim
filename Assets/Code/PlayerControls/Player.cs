using AI;
using Combat;
using UnityEngine;
using VFX;
using MainCursor = Controls.MainCursor;

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

        [SerializeField] private MainCursor _cursor;

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerVFX _vfx;
        [SerializeField] private AimCurve _aimCurve;

        private Health _health;
        private Stats _stats;

        #endregion Fields

        #region Properties

        public Stats Stats => _stats;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerVFX VFX => _vfx;
        public AimCurve AimCurve => _aimCurve;

        public static Player Current { get; set; }
        public static Vector3 Position => Current.transform.position;

        #endregion Properties

        #region Unity

        private void Awake()
        {
            Current = this;
            InitializeStats();
            InitializeHealth();
        }

        #endregion Unity

        #region Private

        private void InitializeStats()
        {
            _stats = GetComponent<Stats>();
            _stats.SkillLevelIncreased.AddListener(SpawnSkillEffect);
        }

        private void SpawnSkillEffect(string name, Skill skill)
        {
            Effect.Spawn("SkillLevelUp", Vector3.up, transform);
        }

        private void InitializeHealth()
        {
            _health = GetComponent<Health>();
            _health.Receiver = this;
            _health.GotHit.AddListener(OnGotHit);
        }

        private void OnGotHit(Attack attack)
        {
            _stats.CurrentHP -= attack.Damage;
            if (_stats.CurrentHP <= 0F)
            {
                Debug.Log("Zabito gracza.");
                _stats.CurrentHP = _stats.MaxHP;
            }
        }

        #endregion Private
    }
}