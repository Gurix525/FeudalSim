using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public abstract class Sense : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        protected float _perceptingDistance = 10F;

        #endregion Fields

        #region Properties

        public UnityEvent PerceptingDistanceChanged { get; } = new();

        public float MaxPerceptingDistance
        {
            get => _perceptingDistance;
            private set
            {
                _perceptingDistance = value;
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
            if (gameObject == null)
                return false;
            return Vector3.Distance(
                transform.position, gameObject.transform.position)
                <= _perceptingDistance;
        }

        #endregion Protected
    }
}