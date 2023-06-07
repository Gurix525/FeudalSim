using UnityEngine;

namespace StateMachineBehaviours
{
    public class ForceNormalMotion : StateMachineBehaviour
    {
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.applyRootMotion = false;
        }
    }
}