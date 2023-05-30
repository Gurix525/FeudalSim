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

        public Vector3 Destination => NavAgent.destination;

        public float Speed { get => NavAgent.speed; set => NavAgent.speed = value; }

        public float Acceleration { get => NavAgent.acceleration; set => NavAgent.acceleration = value; }

        private NavMeshAgent NavAgent => _agent ??= GetComponent<NavMeshAgent>();

        #endregion Properties

        #region Public

        public void Disable()
        {
            _agent.enabled = false;
        }

        public void Enable()
        {
            _agent.enabled = true;
        }

        public void SetDestination(Vector3 targetPosition, bool hasToSamplePosition = true)
        {
            if (hasToSamplePosition)
            {
                NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);
                targetPosition = hit.position;
            }
            NavAgent.SetDestination(targetPosition);
        }

        public void Stop()
        {
            NavAgent.isStopped = true;
        }

        public void Resume()
        {
            NavAgent.isStopped = false;
        }

        public void ResetPath()
        {
            NavAgent.ResetPath();
        }

        public void Move(Vector3 offset)
        {
            NavAgent.Move(offset);
        }

        #endregion Public
    }
}