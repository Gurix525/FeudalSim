using Extensions;
using UnityEngine;

namespace AI
{
    [DisallowMultipleComponent]
    public class Sight : Sense
    {
        [Range(0F, 360F)][SerializeField] private float _fieldOfViewAngle = 90F;

        public override bool IsObjectPerceptible(GameObject other)
        {
            return Vector3.Dot(
                transform.forward,
                (other.transform.position - transform.position).normalized)
                > -(_fieldOfViewAngle / 360F).Remap(0F, 1F, -1F, 1F)
                && (transform.position - other.transform.position).magnitude
                < _perceptingDistance;
        }
    }
}