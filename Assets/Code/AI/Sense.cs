using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public abstract class Sense : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _maxPerceptingDistance = 20F;

        #endregion Fields

        #region Properties

        public UnityEvent PerceptingDistanceChanged { get; } = new();

        public float MaxPerceptingDistance
        {
            get => _maxPerceptingDistance;
            private set
            {
                _maxPerceptingDistance = value;
                PerceptingDistanceChanged.Invoke();
            }
        }

        #endregion Properties

        #region Public

        public abstract bool IsObjectPerceptible(GameObject gameObject);

        #endregion Public

        #region Protected

        protected bool IsObjectInPerceptingRange(GameObject gameObject)
        {
            return Vector3.Distance(
                transform.position, gameObject.transform.position)
                <= _maxPerceptingDistance;
        }

        #endregion Protected
    }
}