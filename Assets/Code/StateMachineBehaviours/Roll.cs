using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class Roll : StateMachineBehaviour
    {
        public UnityEvent<bool> RollPending { get; } = new();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RollPending.Invoke(true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RollPending.Invoke(false);
        }
    }
}