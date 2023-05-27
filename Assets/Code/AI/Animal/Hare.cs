using System;
using System.Collections;
using UnityEngine;

namespace AI
{
    public class Hare : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Wolf), AttitudeType.Scared, () => 100F));
            AddAttitude((typeof(Hare), AttitudeType.Friendly, () => -10F));
        }

        public class ScaredBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(RunAway);
            }

            private void OnEnable()
            {
                Agent.Speed = 8F;
                Agent.Acceleration = 8F;
            }

            private IEnumerator RunAway()
            {
                float MaxRunTime = 5F;
                float elapsedRunTime = 0F;
                Vector3 runDirection =
                    (transform.position - Focus.transform.position).normalized * 50F;
                Agent.SetDestination(transform.position + runDirection);
                while (elapsedRunTime < MaxRunTime)
                {
                    elapsedRunTime += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
            }

            private void OnEnable()
            {
                Agent.Speed = 2F;
                Agent.Acceleration = 2F;
            }
        }
    }
}