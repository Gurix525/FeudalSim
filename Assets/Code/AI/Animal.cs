using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(EntitiesDetector))]
    public abstract class Animal : MonoBehaviour, IDetectable
    {
        #region Fields

        protected EntitiesDetector _detector;
        protected Dictionary<Component, float> _interests = new();

        #endregion Fields

        #region Unity

        protected virtual void Awake()
        {
            _detector = GetComponent<EntitiesDetector>();
            _detector.DetectableBecameVisible.AddListener(OnEntityDetected);
            _detector.DetectableBecameInvisible.AddListener(OnEntityDetectionLost);
        }

        #endregion Unity

        #region Protected

        protected abstract void OnEntityDetected(Component component);

        protected virtual void OnEntityDetectionLost(Component component)
        {
            _interests.Remove(component);
        }

        #endregion Protected
    }
}