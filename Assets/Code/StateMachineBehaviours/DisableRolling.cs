using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class DisableRolling : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsRolling", false);
        }
    }
}