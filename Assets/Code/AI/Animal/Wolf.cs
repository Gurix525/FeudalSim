using System.Collections;
using Combat;
using Controls;
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
            AddAttitude((typeof(Animal), AttitudeType.Hostile,
                (target) => 100F + GetDistancePoints(target)));
            AddAttitude((typeof(Player), AttitudeType.Hostile,
                (target) => 100F + GetDistancePoints(target)));
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
                while ((Vector3.Dot(transform.forward,
                    (Focus.transform.position - transform.position).normalized) < 0.9F
                    || Vector3.Distance(transform.position, Focus.transform.position) >= 4F)
                    && !hasToUpdate)
                {
                    Agent.SetDestination(Focus.transform.position);
                    yield return new WaitForFixedUpdate();
                }
                StateUpdated.RemoveAllListeners();
            }

            private IEnumerator AttackTarget()
            {
                float distance = Vector3.Distance(transform.position, Focus.transform.position);
                if (distance >= 4F)
                    yield break;
                if (Vector3.Dot(
                    transform.forward,
                    (Focus.transform.position - transform.position).normalized)
                    < 0.9F)
                    yield break;
                yield return new WaitForSeconds(0.5F);
                Animal.SetAttackActive(true);
                Animal.SetAttackTarget(Focus);
                Vector3 direction = (Focus.transform.position - transform.position).normalized * Agent.Speed;
                Agent.ResetPath();
                float delta = 0F;
                bool hasHit = false;
                Animal.DealedHit.AddListener((hitbox) => hasHit = true);
                StateUpdated.AddListener(
                    () =>
                    {
                        Animal.DealedHit.RemoveAllListeners();
                        Animal.SetAttackTarget(null);
                    });
                while (delta < distance && hasHit == false)
                {
                    Vector3 movement = direction * Time.fixedDeltaTime;
                    Agent.Move(movement);
                    delta += movement.magnitude;
                    yield return new WaitForFixedUpdate();
                }
                Animal.SetAttackActive(false);
                Animal.SetAttackTarget(null);
                yield return new WaitForSeconds(1F);
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