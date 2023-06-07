using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class DisableAttacking : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsAttacking", false);
        }
    }
}