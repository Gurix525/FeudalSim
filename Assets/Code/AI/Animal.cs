using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntitiesDetector), typeof(Agent))]
    public abstract class Animal : MonoBehaviour, IDetectable
    {
        #region Fields

        private EntitiesDetector _detector;
        private Agent _agent;
        private Dictionary<Component, Attitude> _attitudes = new();
        private List<AttitudeModel> _attitudeModels = new();
        private Dictionary<AttitudeType, AIBehaviour> _behaviours = new();
        private Attitude _highestPriorityAttitude;
        private float _attitudesCheckInterval = 5F;
        private float _timeSinceAttitudesCheck;

        #endregion Fields

        #region Properties

        public Component Focus => HighestPriorityAttitude?.Component;
        public IReadOnlyDictionary<Component, Attitude> Attitudes => _attitudes;
        public float MaxDetectingDistance => _detector.MaxDetectingDistance;

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
            _detector = GetComponent<EntitiesDetector>();
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
            CreateAttitudeModels();
            CreateBehaviours();
            RecalculateAttitudesAndSelectBehaviour();
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
            _attitudes.Add(component, new(component, type, strengthCalculationMethod));
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
            _attitudes.Remove(component);
            RecalculateAttitudesAndSelectBehaviour();
        }

        private void RecalculateAttitudesAndSelectBehaviour()
        {
            _timeSinceAttitudesCheck = 0F;
            if (_attitudes.Count == 0)
            {
                HighestPriorityAttitude = null;
                ChangeBehavioursState(AttitudeType.Neutral);
                return;
            }
            foreach (var attitude in _attitudes)
                attitude.Value.RecalculatePower();
            HighestPriorityAttitude = _attitudes.Values.Aggregate(
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

        private void CheckAttitudes()
        {
            _timeSinceAttitudesCheck += Time.fixedDeltaTime;
            if (_timeSinceAttitudesCheck >= _attitudesCheckInterval)
            {
                RecalculateAttitudesAndSelectBehaviour();
            }
        }

        #endregion Private
    }
}