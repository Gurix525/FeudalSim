using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace AI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntitiesDetector))]
    public abstract class Animal : MonoBehaviour, IDetectable
    {
        #region Fields

        private EntitiesDetector _detector;
        private Dictionary<Component, Attitude> _attitudes = new();
        private List<AttitudeModel> _attitudeModels = new();
        private Dictionary<AttitudeType, AIBehaviour> _behaviours = new();

        #endregion Fields

        #region Public

        public AIBehaviour AddBehaviour<T>() where T : AIBehaviour
        {
            return gameObject.AddComponent<T>();
        }

        #endregion Public

        #region Unity

        protected void Awake()
        {
            _detector = GetComponent<EntitiesDetector>();
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
            CreateAttitudeModels();
            CreateBehaviours();
        }

        #endregion Unity

        #region Protected

        protected abstract void CreateAttitudeModels();

        protected virtual void OnEntityDetected(Component component)
        {
            foreach (var attitudeModel in _attitudeModels)
            {
                if (component.GetType().IsSameOrSubclass(attitudeModel.Type))
                {
                    AddAttitude(component, attitudeModel.AttitudeType,
                        attitudeModel.Method);
                    return;
                }
            }
        }

        protected virtual void OnEntityDetectionLost(Component component)
        {
            _attitudes.Remove(component);
        }

        protected void AddAttitude(AttitudeModel model)
        {
            _attitudeModels.Add(model);
        }

        #endregion Protected

        #region Private

        private void AddAttitude(
            Component component,
            AttitudeType type,
            Func<float> strengthCalculationMethod)
        {
            _attitudes.Add(component, new(type, strengthCalculationMethod));
        }

        private void CreateBehaviours()
        {
            DateTime t1 = DateTime.Now;
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
                }
            DateTime t2 = DateTime.Now;
            Debug.Log((t2 - t1).TotalMilliseconds);
        }

        #endregion Private
    }
}