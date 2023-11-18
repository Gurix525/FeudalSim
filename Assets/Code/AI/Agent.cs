using UnityEngine;
using UnityEngine.AI;
using World;

namespace AI
{
    public class Agent : MonoBehaviour
    {
        #region Fields

        private NavMeshAgent _agent;
        private TerrainRenderer _terrainRenderer;

        #endregion Fields

        #region Properties

        public bool IsActive => _agent.isActiveAndEnabled;

        public Vector3 Destination => NavAgent.destination;

        public Vector3 Velocity { get => NavAgent.velocity; set => NavAgent.velocity = value; }

        public float Speed { get => NavAgent.speed; set => NavAgent.speed = value; }

        public float Acceleration { get => NavAgent.acceleration; set => NavAgent.acceleration = value; }

        public float AngularSpeed { get => NavAgent.angularSpeed; set => NavAgent.angularSpeed = value; }

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
                NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 10F, NavMesh.AllAreas);
                targetPosition = hit.position;
            }
            if (NavAgent.isOnNavMesh)
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
            if (!IsActive)
                return;
            if (!NavAgent.isOnNavMesh)
                return;
            NavAgent.Move(offset);
        }

        #endregion Public

        #region Unity

        private void OnEnable()
        {
            _terrainRenderer ??= FindObjectOfType<TerrainRenderer>();
            _terrainRenderer.NavMeshRebaked += _terrainRenderer_NavMeshRebaked;
        }

        private void OnDisable()
        {
            _terrainRenderer.NavMeshRebaked -= _terrainRenderer_NavMeshRebaked;
        }

        #endregion Unity

        #region Private

        private void _terrainRenderer_NavMeshRebaked(object sender, System.EventArgs e)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1F, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                Enable();
            }
            else
                Disable();
        }

        #endregion Private
    }
}