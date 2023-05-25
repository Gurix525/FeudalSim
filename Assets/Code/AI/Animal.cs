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
        protected List<(Type type, AttitudeType attitudeType, Func<float> method)> _attitudesMap = new();

        #endregion Fields

        #region Unity

        protected virtual void Awake()
        {
            _detector = GetComponent<EntitiesDetector>();
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
            CreateAttitudesMap();
        }

        #endregion Unity

        #region Protected

        protected virtual void OnEntityDetected(Component component)
        {
            foreach (var attitudeModel in _attitudesMap)
            {
                if (attitudeModel.type == component.GetType())
                {
                    AddAttitude(component, attitudeModel.attitudeType,
                        attitudeModel.method);
                    return;
                }
            }
        }

        protected virtual void OnEntityDetectionLost(Component component)
        {
            _attitudes.Remove(component);
        }

        protected abstract void CreateAttitudesMap();

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