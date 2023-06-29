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
        private Func<bool> _getStopCondition;

        #endregion Fields

        #region Properties

        public float Power => GetPower();

        public Task Task { get; private set; }
        public UnityEvent TaskFinished { get; } = new();

        #endregion Properties

        #region Public

        public AIAction(Func<IEnumerator> getCoroutine, Func<float> getPower, Func<bool> getStopCondition)
        {
            _getCoroutine = getCoroutine;
            _getPower = getPower;
            _getStopCondition = getStopCondition;
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
            Task = new(GetConditionalCoroutine(), false);
            Task.Finished += (manual) => TaskFinished.Invoke();
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, Func<float> getPower, Func<bool> getStopCondition) input)
        {
            return new(input.getCoroutine, input.getPower, input.getStopCondition);
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, float power, Func<bool> getStopCondition) input)
        {
            return new(input.getCoroutine, () => input.power, input.getStopCondition);
        }

        public static implicit operator AIAction((Func<IEnumerator> getCoroutine, Func<bool> getStopCondition) input)
        {
            return new(input.getCoroutine, () => 1F, input.getStopCondition);
        }

        #endregion Public

        #region Private

        private float GetPower()
        {
            return _getStopCondition() ? 0F : _getPower();
        }

        private IEnumerator GetConditionalCoroutine()
        {
            var coroutine = _getCoroutine();
            while (true)
            {
                if (_getStopCondition())
                    yield break;
                if (coroutine.MoveNext())
                    yield return coroutine.Current;
                else yield break;
            }
        }

        #endregion Private
    }
}