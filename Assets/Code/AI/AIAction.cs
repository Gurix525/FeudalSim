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
        private Func<float> _getPower;

        #endregion Fields

        #region Properties

        public float Power => _getPower();
        public Task Task { get; private set; }
        public UnityEvent TaskFinished { get; } = new();

        #endregion Properties

        #region Public

        public AIAction(Func<IEnumerator> getCoroutine, Func<float> getPower)
        {
            _getCoroutine = getCoroutine;
            _getPower = getPower;
            ResetTask(false);
        }

        public void StartTask()
        {
            Task.Start();
        }

        public void StopTask()
        {
            Task.Stop();
        }

        public void ResetTask(bool manual)
        {
            if (Task != null)
                Task.Stop();
            TaskFinished.RemoveAllListeners();
            Task = new(_getCoroutine(), false);
            Task.Finished += (manual) => TaskFinished.Invoke();
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, Func<float> getPower) input)
        {
            return new(input.getCoroutine, input.getPower);
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, float power) input)
        {
            return new(input.getCoroutine, () => input.power);
        }

        public static implicit operator AIAction(Func<IEnumerator> getCoroutine)
        {
            return new(getCoroutine, () => 1F);
        }

        #endregion Public
    }
}