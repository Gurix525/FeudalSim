using UnityEngine;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void OnEntityDetected(Component component)
        {
            if (component is Wolf)
                _interests[component] = 100F;
            Debug.Log(_interests[component]);
        }
    }
}