using System.Collections;
using Combat;
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
                AddAction(AttackTarget);
            }

            protected override void DuringEnable()
            {
                SetSpeed(MoveSpeedType.Run);
            }

            private IEnumerator ChaseTarget()
            {
                bool hasToUpdate = false;
                StateUpdated.AddListener(() => hasToUpdate = true);
                while (Vector3.Distance(transform.position, Focus.transform.position) >= 4F
                    && !hasToUpdate)
                {
                    Agent.SetDestination(Focus.transform.position);
                    yield return new WaitForFixedUpdate();
                }
                StateUpdated.RemoveAllListeners();
            }

            private IEnumerator AttackTarget()
            {
                if (Vector3.Distance(transform.position, Focus.transform.position) >= 4F)
                    yield break;
                float oldSpeed = Agent.Speed;
                Agent.Speed *= 2F;
                var attack = Attack.Spawn(this, Vector3.forward, 4F, lifetime: 0.5F, parent: transform);
                while (attack.gameObject.activeSelf)
                {
                    Agent.SetDestination(Focus.transform.position);
                    yield return new WaitForFixedUpdate();
                }
                Agent.Speed = oldSpeed;
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction((StandIdle, 1F));
                AddAction((Roam, 2F));
            }

            protected override void DuringEnable()
            {
                SetSpeed(MoveSpeedType.Walk);
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