using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AI
{
    public class EntitiesDetector : MonoBehaviour
    {
        #region Fields

        private SphereCollider _sphereCollider;
        private List<Component> _notVisibleDetectables = new();
        private ObservableCollection<Component> _visibleDetectables = new();

        #endregion Fields

        #region Public

        public void SetDetectingRadius(float radius)
        {
            _sphereCollider.radius = radius;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _visibleDetectables.CollectionChanged += (a, b) => Debug.Log(b.NewItems + " " + b.OldItems);
        }

        private void FixedUpdate()
        {
            foreach (var component in _notVisibleDetectables)
            {
                if (true)
                {
                    _notVisibleDetectables.Remove(component);
                    _visibleDetectables.Add(component);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDetectable detectable))
                _notVisibleDetectables.Add((Component)detectable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDetectable detectable))
            {
                Component component = (Component)detectable;
                _notVisibleDetectables.Remove(component);
                _visibleDetectables.Remove(component);
            }
        }

        #endregion Unity
    }
}