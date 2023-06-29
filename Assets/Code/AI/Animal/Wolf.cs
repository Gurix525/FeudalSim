using PlayerControls;
using System.Collections;
using Extensions;
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
                AddAction(ChaseTarget, GetChaseTargetPoints);
                AddAction(RotateToTarget, GetRotateToTargetPoints);
                AddAction(AttackTarget, GetAttackTargetPoints);
            }

            protected override void DuringEnable()
            {
                SetSpeed(MoveSpeedType.Chase);
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

            private IEnumerator RotateToTarget()
            {
                while (Vector3.Dot(
                    transform.forward,
                    (Focus.transform.position - transform.position).normalized)
                    < 0.99F)
                {
                    Vector3 direction = Focus.transform.position - transform.position;
                    if (direction.magnitude >= 4F)
                        yield break;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5);
                    yield return new WaitForFixedUpdate();
                }
            }

            private IEnumerator AttackTarget()
            {
                float startDistanceToTarget = (transform.position - Focus.transform.position).magnitude;
                Animal.SetAttackActive(true);
                Animal.SetAttackTarget(Focus);
                Vector3 direction = (Focus.transform.position - transform.position).normalized * Agent.Speed;
                Agent.ResetPath();
                float distanceFromStart = 0F;
                bool hasHit = false;
                Animal.DealedHit.AddListener((hitbox) => hasHit = true);
                StateUpdated.AddListener(
                    () =>
                    {
                        Animal.DealedHit.RemoveAllListeners();
                        Animal.SetAttackTarget(null);
                    });
                Vector3 startPosition = transform.position;
                while (distanceFromStart < startDistanceToTarget && hasHit == false)
                {
                    Vector3 movement = direction * Time.fixedDeltaTime;
                    Agent.Move(movement);
                    distanceFromStart = (startPosition - transform.position).magnitude;
                    yield return new WaitForFixedUpdate();
                }
                Agent.Velocity = Vector3.zero;
                Animal.SetAttackActive(false);
                Animal.SetAttackTarget(null);
                yield return new WaitForSeconds(0.5F);
                StateUpdated.RemoveAllListeners();
            }

            private float GetChaseTargetPoints()
            {
                return (Focus.transform.position - transform.position)
                    .magnitude >= 4F ? 1F : 0F;
            }

            private float GetRotateToTargetPoints()
            {
                return ((Focus.transform.position - transform.position)
                    .magnitude < 4F ? 1F : 0F) *
                    Vector3.Dot(
                    transform.forward,
                    (Focus.transform.position - transform.position).normalized)
                    < 0.99F ? 1 : 0;
            }

            private float GetAttackTargetPoints()
            {
                return ((Focus.transform.position - transform.position)
                    .magnitude < 4F ? 1F : 0F) *
                    Vector3.Dot(
                    transform.forward,
                    (Focus.transform.position - transform.position).normalized)
                    >= 0.99F ? 1 : 0;
            }
        }

        public class NeutralBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(StandIdle, 1F);
                AddAction(Roam, 2F);
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