using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class AttackStartStop : StateMachineBehaviour
    {
        public UnityEvent<bool> PendingAttack { get; } = new();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            new TaskManager.Task(InvokeTrue());
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            PendingAttack.Invoke(false);
        }

        private IEnumerator InvokeTrue()
        {
            yield return null;
            PendingAttack.Invoke(true);
        }
    }
}