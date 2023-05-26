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

        protected override void CreateBehaviours()
        {
            AddBehaviour<FriendlyBehaviour>(AttitudeType.Friendly);
            AddBehaviour<HostileBehaviour>(AttitudeType.Hostile);
            AddBehaviour<ScaredBehaviour>(AttitudeType.Scared);
            AddBehaviour<HungryBehaviour>(AttitudeType.Hungry);
            AddBehaviour<NeutralBehaviour>(AttitudeType.Neutral);
        }

        private class FriendlyBehaviour : AIBehaviour
        { }

        private class HostileBehaviour : AIBehaviour
        { }

        private class ScaredBehaviour : AIBehaviour
        { }

        private class HungryBehaviour : AIBehaviour
        { }

        private class NeutralBehaviour : AIBehaviour
        { }
    }
}