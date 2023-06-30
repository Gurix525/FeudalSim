using System.Collections;
using System.Linq;
using Extensions;
using UnityEngine;
using PlayerControls;

namespace AI
{
    public class Hare : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Wolf), AttitudeType.Scared, (target) => 100F));
            AddAttitude((typeof(Player), AttitudeType.Scared, (target) => 100F));
            AddAttitude((typeof(Hare), AttitudeType.Friendly, (target) => -10F));
        }

        public class ScaredBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(RunAway);
            }

            protected override void DuringEnable()
            {
                SetSpeed(MoveSpeedType.RunAway);
            }

            private IEnumerator RunAway()
            {
                bool hasToUpdate = false;
                StateUpdated.AddListener(() => hasToUpdate = true);
                Vector3 runDirection = Animal.Attitudes
                    .Where(attitude => attitude.AttitudeType == AttitudeType.Scared && attitude.Component != null)
                    .Select(attitude => (transform.position - attitude.Component.transform.position).normalized)
                    .Aggregate((last, current) => (last + current).normalized)
                    .normalized * 10F;
                Agent.SetDestination(transform.position
                    + Quaternion.Euler(0F, _random.NextFloat(-30F, 30F), 0F) * runDirection);
                yield return new WaitUntil(() =>
                {
                    if (this == null)
                        return true;
                    return Vector3.Distance(transform.position, Agent.Destination) < 2F
                    || hasToUpdate;
                });
                StateUpdated.RemoveAllListeners();
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
                while (this == null ? true : Vector3.Distance(transform.position, Agent.Destination) > 2F
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