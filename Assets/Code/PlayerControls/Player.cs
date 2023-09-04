using System;
using AI;
using Combat;
using Controls;
using Extensions;
using Input;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;
using VFX;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;

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

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerVFX _vfx;
        [SerializeField] private AimCurve _aimCurve;

        [SerializeField] private RightHandItemHook _rightHandItemHook;

        [SerializeField] private LeftHandItemHook _leftHandItemHook;

        private Health _health;
        private Stats _stats;

        #endregion Fields

        #region Properties

        public static Player Instance { get; set; }

        public Stats Stats => _stats;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerVFX VFX => _vfx;
        public AimCurve AimCurve => _aimCurve;

        public LeftHandItemHook LeftHandItemHook => _leftHandItemHook;
        public RightHandItemHook RightHandItemHook => _rightHandItemHook;

        public static Vector3 Position => Instance.transform.position;

        #endregion Properties

        #region Unity

        private void Awake()
        {
            Instance = this;
            InitializeStats();
            InitializeHealth();
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
            //Cursor.Action.Update();
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
            //Cursor.Action.OnLeftMouseButton();
        }

        private void OnLeftMouseButtonRelase(CallbackContext context)
        {
            //Cursor.Action.OnLeftMouseButtonRelase();
        }

        private void OnRightMouseButton(CallbackContext context)
        {
            //Cursor.Action.OnRightMouseButton();
        }

        private void OnRightMouseButtonRelase(CallbackContext context)
        {
            //Cursor.Action.OnRightMouseButtonRelase();
        }

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