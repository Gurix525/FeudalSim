using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class ForceRootMotion : StateMachineBehaviour
    {
        public UnityEvent<bool> RootMotionForced { get; } = new();

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animator.applyRootMotion = true;
            RootMotionForced.Invoke(true);
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            animator.applyRootMotion = false;
            RootMotionForced.Invoke(false);
        }
    }
}