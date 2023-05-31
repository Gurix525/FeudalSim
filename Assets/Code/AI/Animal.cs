using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Extensions;
using TaskManager;
using UnityEngine;

namespace AI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntitiesDetector), typeof(Agent), typeof(Health))]
    public abstract class Animal : MonoBehaviour, IDetectable
    {
        #region Fields

        private EntitiesDetector _detector;
        private Agent _agent;
        private Health _health;
        private List<Attitude> _attitudes = new();
        private List<AttitudeModel> _attitudeModels = new();
        private Dictionary<AttitudeType, AIBehaviour> _behaviours = new();
        private Attitude _highestPriorityAttitude;
        private float _attitudesCheckInterval = 5F;
        private float _timeSinceAttitudesCheck;
        private MoveSpeedType _moveSpeedType;
        private bool _isKnockbackActive;
        private bool _isBeingDestroyed;
        private Task _knockbackTask;

        private Dictionary<MoveSpeedType, MoveSpeed> _moveSpeeds = new()
        {
            { MoveSpeedType.Walk, new(2F, 2F)},
            { MoveSpeedType.Trot, new(4F, 4F) },
            { MoveSpeedType.Run, new(8F, 8F) }
        };

        #endregion Fields

        #region Properties

        public Component Focus => HighestPriorityAttitude?.Component ?? this;

        public IReadOnlyCollection<Attitude> Attitudes => _attitudes;

        public float MaxDetectingDistance => _detector.MaxDetectingDistance;

        public MoveSpeedType MoveSpeed
        {
            get => _moveSpeedType;
            set
            {
                _moveSpeedType = value;
                SetSpeed(value);
            }
        }

        private Attitude HighestPriorityAttitude
        {
            get => _highestPriorityAttitude;
            set
            {
                if (_highestPriorityAttitude == value)
                    return;
                _highestPriorityAttitude = value;
                _agent.ResetPath();
            }
        }

        private bool AreBehavioursPermitted => !_isKnockbackActive && !_isBeingDestroyed;

        #endregion Properties

        #region Public

        public AIBehaviour AddBehaviour<T>() where T : AIBehaviour
        {
            return gameObject.AddComponent<T>();
        }

        #endregion Public

        #region Unity

        protected void Awake()
        {
            _agent = GetComponent<Agent>();
            RandomizeSpeedValues();
            SetSpeed(MoveSpeedType.Walk);
            _detector = GetComponent<EntitiesDetector>();
            _health = GetComponent<Health>();
            _health.GotHit.AddListener(OnGotHit);
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
            CreateAttitudeModels();
            CreateBehaviours();
            RecalculateAttitudesAndSelectBehaviour();
        }

        private void OnDisable()
        {
            DisableAllBehaviours();
        }

        private void FixedUpdate()
        {
            CheckAttitudes();
        }

        #endregion Unity

        #region Protected

        protected abstract void CreateAttitudeModels();

        protected void AddAttitude(AttitudeModel model)
        {
            _attitudeModels.Add(model);
        }

        #endregion Protected

        #region Private

        private void AddAttitude(
            Component component,
            AttitudeType type,
            Func<Component, float> strengthCalculationMethod)
        {
            _attitudes.Add(new(component, type, strengthCalculationMethod));
        }

        private void CreateBehaviours()
        {
            var thisType = GetType();
            var nestedTypes = thisType.GetNestedTypes();
            if (nestedTypes.Length == 0)
                return;
            var method = thisType.GetMethod(nameof(AddBehaviour));
            foreach (var type in nestedTypes)
                if (type.IsSubclassOf(typeof(AIBehaviour)))
                {
                    AttitudeType attitudeType = type.Name switch
                    {
                        "FriendlyBehaviour" => AttitudeType.Friendly,
                        "HostileBehaviour" => AttitudeType.Hostile,
                        "ScaredBehaviour" => AttitudeType.Scared,
                        "HungryBehaviour" => AttitudeType.Hungry,
                        _ => AttitudeType.Neutral
                    };
                    var generic = method.MakeGenericMethod(type);
                    _behaviours[attitudeType] = (AIBehaviour)generic.Invoke(this, null);
                    _behaviours[attitudeType].Animal = this;
                    _behaviours[attitudeType].Agent = _agent;
                }
        }

        private void OnEntityDetected(Component component)
        {
            foreach (var attitudeModel in _attitudeModels)
            {
                if (component.GetType().IsSameOrSubclass(attitudeModel.Type))
                {
                    AddAttitude(component, attitudeModel.AttitudeType,
                        attitudeModel.Method);
                    break;
                }
            }
            RecalculateAttitudesAndSelectBehaviour();
        }

        private void OnEntityDetectionLost(Component component)
        {
            _attitudes.Remove(_attitudes.Find(attitude => attitude.Component == component));
            RecalculateAttitudesAndSelectBehaviour();
        }

        private void RecalculateAttitudesAndSelectBehaviour()
        {
            if (!AreBehavioursPermitted)
                return;
            _timeSinceAttitudesCheck = 0F;
            if (_attitudes.Count == 0)
            {
                HighestPriorityAttitude = null;
                ChangeBehavioursState(AttitudeType.Neutral);
                return;
            }
            foreach (var attitude in _attitudes)
            {
                attitude.RecalculatePower();
            }
            for (int i = 0; i < _attitudes.Count; i++)
                if (_attitudes[i].Component == null || !_attitudes[i].Component.gameObject.activeInHierarchy)
                {
                    _attitudes.Remove(_attitudes[i]);
                    i--;
                }
            if (_attitudes.Count == 0)
            {
                HighestPriorityAttitude = null;
                ChangeBehavioursState(AttitudeType.Neutral);
                return;
            }
            HighestPriorityAttitude = _attitudes.Aggregate(
                (currentMax, attitude) =>
                attitude.Power > (currentMax ?? attitude).Power
                ? attitude : currentMax);
            if (HighestPriorityAttitude.Power < 0F)
            {
                HighestPriorityAttitude = null;
                ChangeBehavioursState(AttitudeType.Neutral);
                return;
            }
            ChangeBehavioursState(HighestPriorityAttitude.AttitudeType);
        }

        private void ChangeBehavioursState(AttitudeType activeType)
        {
            foreach (var behaviour in _behaviours)
            {
                if (behaviour.Key == activeType)
                    behaviour.Value.enabled = true;
                else
                {
                    behaviour.Value.StopAction();
                    behaviour.Value.enabled = false;
                }
                behaviour.Value.StateUpdated.Invoke();
            }
        }

        private void DisableAllBehaviours()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.Value.StopAction();
                behaviour.Value.enabled = false;
                behaviour.Value.StateUpdated.Invoke();
            }
        }

        private void CheckAttitudes()
        {
            _timeSinceAttitudesCheck += Time.fixedDeltaTime;
            if (_timeSinceAttitudesCheck >= _attitudesCheckInterval)
            {
                RecalculateAttitudesAndSelectBehaviour();
            }
        }

        private void RandomizeSpeedValues()
        {
            System.Random random = new();
            _moveSpeeds = new()
            {
                { MoveSpeedType.Walk, (random.NextFloat(1.8F, 2.2F), random.NextFloat(1.8F, 2.2F))},
                { MoveSpeedType.Trot, (random.NextFloat(3.8F, 4.2F), random.NextFloat(3.5F, 4.2F))},
                { MoveSpeedType.Run, (random.NextFloat(7.8F, 8.2F), random.NextFloat(7F, 8.2F))},
            };
        }

        private void SetSpeed(MoveSpeedType value)
        {
            _agent.Speed = _moveSpeeds[value].Speed;
            _agent.Acceleration = _moveSpeeds[value].Acceleration;
        }

        private void OnGotHit(Attack attack)
        {
            _knockbackTask?.Stop();
            if (_health.CurrentHealth <= 0F)
            {
                new Task(DestroySafely());
                return;
            }
            _knockbackTask = new(KnockBack(attack));
        }

        private IEnumerator KnockBack(Attack attack)
        {
            _isKnockbackActive = true;
            DisableAllBehaviours();
            float elapsedTime = 0F;
            float blockedTime = 0.5F;
            Vector3 direction = (transform.position - attack.transform.position).normalized * 10F;
            while (elapsedTime < blockedTime)
            {
                elapsedTime += Time.fixedDeltaTime;
                _agent.Move(direction * Time.fixedDeltaTime * (blockedTime - elapsedTime) / blockedTime);
                yield return new WaitForFixedUpdate();
            }
            RecalculateAttitudesAndSelectBehaviour();
            _isKnockbackActive = false;
        }

        private IEnumerator DestroySafely()
        {
            _knockbackTask?.Stop();
            _isBeingDestroyed = true;
            DisableAllBehaviours();
            _agent.Disable();
            transform.position = new(100000F, 100000F, 100000F);
            yield return new WaitForFixedUpdate();
            gameObject.SetActive(false);
            yield return new WaitForFixedUpdate();
            Destroy(gameObject);
        }

        #endregion Private
    }
}