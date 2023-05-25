using UnityEngine;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void CreateAttitudesMap()
        {
            _attitudesMap.Add((typeof(Wolf), AttitudeType.Friendly, () => 10F));
            _attitudesMap.Add((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }
    }
}