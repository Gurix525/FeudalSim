using UnityEngine;

namespace AI
{
    [DisallowMultipleComponent]
    public class Hearing : Sense
    {
        public override bool IsObjectPerceptible(GameObject other)
        {
            return (transform.position - other.transform.position)
                .magnitude < _perceptingDistance;
        }
    }
}