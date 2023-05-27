using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Agent : MonoBehaviour
    {
        #region Fields

        private NavMeshAgent _agent;

        #endregion Fields

        #region Properties

        public Vector3 Destination => _agent.destination;

        #endregion Properties

        #region Public

        public void SetDestination(Vector3 targetPosition, bool hasToSamplePosition = true)
        {
            if (hasToSamplePosition)
            {
                NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);
                targetPosition = hit.position;
            }
            _agent.SetDestination(targetPosition);
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

        public void Move(Vector3 offset)
        {
            _agent.Move(offset);
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