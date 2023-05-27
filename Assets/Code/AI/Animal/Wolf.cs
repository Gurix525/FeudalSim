using System.Collections;
using TaskManager;
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
        {
            protected override void CreateActions()
            {
            }
        }

        public class HostileBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
            }
        }

        public class ScaredBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
            }
        }

        public class HungryBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction((MoveRight, 3F));
                AddAction((MoveLeft, 3F));
            }

            private IEnumerator MoveRight()
            {
                while (true)
                {
                    Agent.Move(transform.right * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
                }
            }

            private IEnumerator MoveLeft()
            {
                while (true)
                {
                    Agent.Move(-transform.right * Time.fixedDeltaTime);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}