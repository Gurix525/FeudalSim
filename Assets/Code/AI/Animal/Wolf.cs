using UnityEngine;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void OnEntityDetected(Component detectedComponent)
        {
            if (detectedComponent is Wolf)
                _interests[detectedComponent] = 100F;
        }
    }
}