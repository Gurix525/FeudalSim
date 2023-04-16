using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controls
{
    public class CollisionCounter : MonoBehaviour
    {
        private MeshCollider _collider;
        private List<Collider> _colliders = new();

        public bool IsColliding { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<MeshCollider>();
        }

        private void FixedUpdate()
        {
            _colliders = _colliders.Where(collider => collider != null).ToList();
            bool isColliding = false;
            foreach (var other in _colliders)
            {
                Physics.ComputePenetration(_collider, transform.position, transform.rotation,
                    other, other.transform.position, other.transform.rotation, out _, out float distance);
                if (distance > 0.05F)
                    isColliding = true;
            }
            IsColliding = isColliding;
        }

        private void OnTriggerEnter(Collider other)
        {
            _colliders.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _colliders.Remove(other);
        }
    }
}