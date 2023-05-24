using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class EntitiesDetector : MonoBehaviour
    {
        #region Fields

        private SphereCollider _sphereCollider;
        private List<Component> _notVisibleDetectables = new();
        private ObservableCollection<Component> _visibleDetectables = new();
        private Sense[] _senses;

        #endregion Fields

        #region Properties

        public UnityEvent<Component> DetectableBecameVisible { get; } = new();

        public UnityEvent<Component> DetectableBecameInvisible { get; } = new();

        #endregion Properties

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
            _senses = GetComponents<Sense>();
            foreach (var sense in _senses)
                sense.PerceptingDistanceChanged.AddListener(AdjustDetectingTriggerRadius);
            AdjustDetectingTriggerRadius();
            _visibleDetectables.CollectionChanged += PublishCollectionChanges;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _notVisibleDetectables.Count; i++)
            {
                if (IsObjectPerceptible(_notVisibleDetectables[i]))
                {
                    _visibleDetectables.Add(_notVisibleDetectables[i]);
                    _notVisibleDetectables.RemoveAt(i);
                    i--;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
                return;
            if (!other.TryGetComponent(out IDetectable detectable))
                return;
            Component component = detectable as Component;
            if (!_visibleDetectables.Contains(component))
                if (!_notVisibleDetectables.Contains(component))
                    _notVisibleDetectables.Add(component);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger)
                return;
            if (!other.TryGetComponent(out IDetectable detectable))
                return;
            Component component = (Component)detectable;
            _notVisibleDetectables.Remove(component);
            _visibleDetectables.Remove(component);
        }

        #endregion Unity

        #region Private

        private void PublishCollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var newItem in e.NewItems)
                    DetectableBecameVisible.Invoke((Component)newItem);
            if (e.OldItems != null)
                foreach (var oldItem in e.OldItems)
                    DetectableBecameInvisible.Invoke((Component)oldItem);
        }

        private void AdjustDetectingTriggerRadius()
        {
            _sphereCollider.radius = _senses.Length > 0F ?
                _senses.Max(sense => sense.MaxPerceptingDistance)
                : 0F;
        }

        private bool IsObjectPerceptible(Component component)
        {
            foreach (var sense in _senses)
            {
                if (sense.IsObjectPerceptible(component.gameObject))
                    return true;
            }
            return false;
        }

        #endregion Private
    }
}