using UnityEngine;

namespace VFX
{
    public class BloodSplatter : Effect
    {
        private static int _groundLayer = LayerMask.NameToLayer("Default");

        private void OnEnable()
        {
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 50F, _groundLayer);
            _visualEffect.SetFloat("CollisionPlaneDistance", hit.distance);
        }
    }
}