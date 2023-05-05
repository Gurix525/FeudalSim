using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extensions;
using UnityEngine;

public class TemporaryRigidbody : MonoBehaviour
{
    private float _mass;
    private Vector3 _startForce;
    private Vector3 _startForcePosition;

    public void ActivateRigidbody(Vector3 startForce, Vector3 startForcePosition, float mass = 1F)
    {
        System.Random random = new();
        float randomAngle = (float)random.NextDouble();
        randomAngle = randomAngle.Remap(0F, 1F, 0F, 360F);
        _mass = mass;
        _startForce = Quaternion.Euler(0F, randomAngle, 0F) * startForce;
        _startForcePosition = startForcePosition;
        _ = DoPhysics();
    }

    private async Task DoPhysics()
    {
        var meshCollider = GetComponent<MeshCollider>();
        meshCollider.convex = true;
        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = _mass;
        rigidbody.AddForceAtPosition(_startForce, _startForcePosition, ForceMode.Impulse);
        while (!rigidbody.IsSleeping())
        {
            await Task.Yield();
        }
        rigidbody.isKinematic = true;
        Destroy(rigidbody);
        meshCollider.convex = false;
    }
}