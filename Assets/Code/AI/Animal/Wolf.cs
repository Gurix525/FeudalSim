using System.Collections;
using Extensions;
using Misc;
using TaskManager;
using UnityEngine;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Wolf), AttitudeType.Friendly, () => -10F));
            AddAttitude((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }

        public class HostileBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(ChaseTarget);
            }

            private void OnEnable()
            {
                Agent.Speed = 8F;
                Agent.Acceleration = 6F;
            }

            private IEnumerator ChaseTarget()
            {
                while (Vector3.Distance(transform.position, Focus.transform.position) > 2F)
                {
                    Agent.SetDestination(Focus.transform.position);
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction((StandIdle, 1F));
                AddAction((Roam, 2F));
            }

            private void OnEnable()
            {
                Agent.Speed = 2F;
                Agent.Acceleration = 2F;
            }

            private IEnumerator StandIdle()
            {
                float randomTime = ((float)_random.NextDouble()).Remap(0F, 1F, 5F, 15F);
                Debug.Log(randomTime);
                yield return new WaitForSeconds(randomTime);
            }

            private IEnumerator Roam()
            {
                float roamMaxTime = _random.NextFloat(5F, 10F);
                float roamingTime = 0F;
                Vector3 randomOffset =
                    Quaternion.Euler(0F, _random.NextFloat(0F, 360F), 0F)
                    * new Vector3(0F, 0F, _random.NextFloat(5F, 20F));
                Agent.SetDestination(transform.position + randomOffset);
                Debug.Log(Agent.Destination);
                while (Vector3.Distance(transform.position, Agent.Destination) > 2F
                    && roamingTime < roamMaxTime)
                {
                    roamingTime += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(roamingTime.Remap(5F, 10F, 2F, 5F));
            }
        }
    }
}