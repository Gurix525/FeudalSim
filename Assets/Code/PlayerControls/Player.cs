using System.Collections;
using Assets;
using Combat;
using UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using VFX;
using World;
using MainCursor = Controls.MainCursor;

namespace PlayerControls
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerVFX))]
    [RequireComponent(typeof(AimCurve))]
    [RequireComponent(typeof(Stats))]
    public class Player : MonoBehaviour
    {
        #region Fields

        [SerializeField] private PlayerInput _input;
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
                StartCoroutine(KillPlayer());
                KillPlayer();
            }
        }

        private IEnumerator KillPlayer()
        {
            AnalyticsBase.Add("entityKilled", "player");
            transform.position = Vector3.zero;
            TerrainRenderer.ForceNavmeshReload();
            //gameObject.SetActive(false);
            LoadingScreen.Enable();
            LoadingScreen.OverrideText("You are dead.\nLoading...");
            _input.DeactivateInput();
            yield return new WaitForSeconds(3F);
            LoadingScreen.Clear();
            LoadingScreen.Disable();
            _input.ActivateInput();
            NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 50F, NavMesh.AllAreas);
            transform.position = hit.position;
            _stats.CurrentHP = _stats.MaxHP;
        }

        #endregion Private
    }
}