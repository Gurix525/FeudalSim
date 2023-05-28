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
            AddAttitude((typeof(Wolf), AttitudeType.Friendly, (target) => -10F));
            AddAttitude((typeof(Animal), AttitudeType.Hostile, (target) =>
                100F + GetDistancePoints(target)));
        }

        private float GetDistancePoints(Component component)
        {
            return 10F - Vector3.Distance(component.transform.position, transform.position)
                .Clamp(0F, MaxDetectingDistance)
                .Remap(0F, MaxDetectingDistance, 0F, 10F);
        }

        public class HostileBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(ChaseTarget);
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                Agent.Speed = 8F;
                Agent.Acceleration = 6F;
            }

            private IEnumerator ChaseTarget()
            {
                bool hasToUpdate = false;
                StateUpdated.AddListener(() => hasToUpdate = true);
                while (Vector3.Distance(transform.position, Focus.transform.position) > 2F
                    && !hasToUpdate)
                {
                    Agent.SetDestination(Focus.transform.position);
                    yield return new WaitForFixedUpdate();
                }
                StateUpdated.RemoveAllListeners();
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction((StandIdle, 1F));
                AddAction((Roam, 2F));
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                Agent.Speed = 2F;
                Agent.Acceleration = 2F;
            }

            private IEnumerator StandIdle()
            {
                float randomTime = ((float)_random.NextDouble()).Remap(0F, 1F, 5F, 15F);
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