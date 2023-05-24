using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            _visibleDetectables.CollectionChanged += PublishCollectionChanges;
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _notVisibleDetectables.Count; i++)
            {
                if (true)
                {
                    _visibleDetectables.Add(_notVisibleDetectables[i]);
                    _notVisibleDetectables.RemoveAt(i);
                    i--;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDetectable detectable))
            {
                Component component = detectable as Component;
                if (!_visibleDetectables.Contains(component))
                    if (!_notVisibleDetectables.Contains(component))
                        _notVisibleDetectables.Add(component);
            }
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

        #region Private

        private void PublishCollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var newItem in e.NewItems)
                DetectableBecameVisible.Invoke((Component)newItem);
            foreach (var oldItem in e.OldItems)
                DetectableBecameInvisible.Invoke((Component)oldItem);
        }

        #endregion Private
    }
}