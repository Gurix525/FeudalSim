using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MyAgent : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        SetRandomTarget();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < 2F)
        {
            SetRandomTarget();
        }
    }

    private void SetRandomTarget()
    {
        var random = RandomVector3.One * 50F;
        random = new(random.x, 0F, random.z);
        NavMesh.SamplePosition(random, out NavMeshHit hit, 300F, NavMesh.AllAreas);
        _targetPosition = hit.position;
        _agent.SetDestination(_targetPosition);
    }
}