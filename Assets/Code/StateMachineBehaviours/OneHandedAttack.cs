using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class OneHandedAttack : StateMachineBehaviour
    {
        public UnityEvent AttackFinished { get; } = new();

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AttackFinished.Invoke();
        }
    }
}