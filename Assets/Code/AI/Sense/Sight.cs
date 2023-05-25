using Extensions;
using UnityEngine;

namespace AI
{
    [DisallowMultipleComponent]
    public class Sight : Sense
    {
        [Range(0F, 360F)][SerializeField] private float _fieldOfViewAngle = 90F;

        public override bool IsObjectPerceptible(GameObject gameObject)
        {
            if (!IsObjectInPerceptingRange(gameObject))
                return false;
            return Vector3.Dot(
                transform.forward,
                (gameObject.transform.position - transform.position).normalized)
                > -(_fieldOfViewAngle / 360F).Remap(0F, 1F, -1F, 1F);
        }
    }
}