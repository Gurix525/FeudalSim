using UnityEngine;
using UnityEngine.Events;

namespace StateMachineBehaviours
{
    public class AttackChange : StateMachineBehaviour
    {
        public UnityEvent AttackChanged { get; } = new();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AttackChanged.Invoke();
        }
    }
}