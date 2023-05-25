using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(EntitiesDetector))]
    public abstract class Animal : MonoBehaviour, IDetectable
    {
        #region Fields

        private EntitiesDetector _detector;
        private Dictionary<Component, Attitude> _attitudes = new();
        protected List<AttitudeModel> _attitudeModels = new();

        #endregion Fields

        #region Unity

        protected virtual void Awake()
        {
            _detector = GetComponent<EntitiesDetector>();
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
            CreateAttitudeModels();
        }

        #endregion Unity

        #region Protected

        protected virtual void OnEntityDetected(Component component)
        {
            foreach (var attitudeModel in _attitudeModels)
            {
                if (attitudeModel.Type == component.GetType())
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

        protected abstract void CreateAttitudeModels();

        #endregion Protected

        #region Private

        private void AddAttitude(
            Component component,
            AttitudeType type,
            Func<float> strengthCalculationMethod)
        {
            _attitudes.Add(component, type switch
            {
                AttitudeType.Friendly => new FriendlyAttitude(strengthCalculationMethod),
                AttitudeType.Hostile => new HostileAttitude(strengthCalculationMethod),
                AttitudeType.Scared => new ScaredAttitude(strengthCalculationMethod),
                AttitudeType.Hungry => new HungryAttitude(strengthCalculationMethod),
                _ => new NeutralAttitude(strengthCalculationMethod),
            });
        }

        #endregion Private
    }
}