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

        public void Stop()
        {
            _agent.isStopped = true;
        }

        public void Resume()
        {
            _agent.isStopped = false;
        }

        public void ResetPath()
        {
            _agent.ResetPath();
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