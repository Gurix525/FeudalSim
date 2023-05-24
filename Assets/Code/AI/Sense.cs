using UnityEngine;
using UnityEngine.Events;

public abstract class Sense : MonoBehaviour
{
    [SerializeField] private float _maxPerceptingDistance = 20F;

    public float MaxPerceptingDistance
    {
        get => _maxPerceptingDistance;
        private set
        {
            _maxPerceptingDistance = value;
        }
    }

    protected bool IsObjectInPerceptingRange(GameObject gameObject)
    {
        return Vector3.Distance(
            transform.position, gameObject.transform.position)
            <= _maxPerceptingDistance;
    }

    public UnityEvent PerceptingDistanceChanged { get; } = new();

    public abstract bool IsObjectPerceptible(GameObject gameObject);
}