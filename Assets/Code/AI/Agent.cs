using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Agent : MonoBehaviour
    {
        #region Fields

        private NavMeshAgent _agent;

        #endregion Fields

        #region Public

        public void SetDestination(Vector3 target)
        {
            _agent.SetDestination(target);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        #endregion Unity
    }
}