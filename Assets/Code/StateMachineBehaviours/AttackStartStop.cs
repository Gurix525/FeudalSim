using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class AttackStartStop : StateMachineBehaviour
    {
        public UnityEvent<bool> PendingAttack { get; } = new();

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            PendingAttack.Invoke(true);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            PendingAttack.Invoke(false);
        }
    }
}