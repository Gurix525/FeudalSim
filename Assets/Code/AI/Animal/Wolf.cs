using UnityEngine;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Wolf), AttitudeType.Friendly, () => 10F));
            AddAttitude((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }

        public class FriendlyBehaviour : AIBehaviour
        { }

        public class HostileBehaviour : AIBehaviour
        { }

        public class ScaredBehaviour : AIBehaviour
        { }

        public class HungryBehaviour : AIBehaviour
        { }

        public class NeutralBehaviour : AIBehaviour
        { }
    }
}