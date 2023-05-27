using System;
using System.Collections;
using TaskManager;
using UnityEngine.Events;

namespace AI
{
    public class AIAction
    {
        #region Fields

        private Func<IEnumerator> _getCoroutine;

        #endregion Fields

        #region Properties

        public float Power { get; set; }
        public Task Task { get; private set; }
        public UnityEvent TaskFinished { get; } = new();

        #endregion Properties

        #region Public

        public AIAction(Func<IEnumerator> getCoroutine, float power)
        {
            Power = power;
            _getCoroutine = getCoroutine;
            ResetTask(false);
        }

        public void StartTask()
        {
            Task.Start();
        }

        public void ResetTask(bool manual)
        {
            if (Task != null)
                Task.Stop();
            Task = new(_getCoroutine(), false);
            Task.Finished += (manual) => TaskFinished.Invoke();
            Task.Finished += ResetTask;
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, float power) input)
        {
            return new(input.getCoroutine, input.power);
        }

        #endregion Public
    }
}