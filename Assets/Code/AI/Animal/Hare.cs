﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace AI
{
    public class Hare : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Wolf), AttitudeType.Scared, (target) => 100F));
            AddAttitude((typeof(Hare), AttitudeType.Friendly, (target) => -10F));
        }

        public class ScaredBehaviour : AIBehaviour
        {
            protected override void CreateActions()
            {
                AddAction(RunAway);
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                Agent.Speed = 8F;
                Agent.Acceleration = 8F;
            }

            private IEnumerator RunAway()
            {
                bool hasToUpdate = false;
                StateUpdated.AddListener(() => hasToUpdate = true);
                Vector3 runDirection = Animal.Attitudes.Values
                    .Where(attitude => attitude.AttitudeType == AttitudeType.Scared)
                    .Select(attitude => (transform.position - attitude.Component.transform.position).normalized)
                    .Aggregate((last, current) => (last + current).normalized)
                    .normalized * 50F;
                Agent.SetDestination(transform.position + runDirection);
                yield return new WaitUntil(() => (
                    Vector3.Distance(transform.position, Agent.Destination) < 10F)
                    || hasToUpdate);
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