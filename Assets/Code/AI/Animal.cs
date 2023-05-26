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

        protected abstract void CreateBehaviours();

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

        protected void AddBehaviour<T>(AttitudeType type) where T : AIBehaviour
        {
            _behaviours[type] = gameObject.AddComponent<T>();
            _behaviours[type].Animal = this;
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

        #endregion Private
    }
}