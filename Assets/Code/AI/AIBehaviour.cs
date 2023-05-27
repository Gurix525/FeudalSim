using System.Collections.Generic;
using Extensions;
using TaskManager;
using UnityEngine;

namespace AI
{
    public abstract class AIBehaviour : MonoBehaviour
    {
        #region Fields

        private AIAction _currentAction;
        private List<AIAction> _actions = new();

        #endregion Fields

        #region Properties

        public Animal Animal { get; set; }
        public Agent Agent { get; set; }

        public Component Focus => Animal.Focus;

        #endregion Properties

        #region Unity

        protected virtual void Awake()
        {
            CreateActions();
            RandomizeAction();
            StartCurrentTask();
            enabled = false;
        }

        private void OnEnable()
        {
            ResetCurrentTask();
            StartCurrentTask();
        }

        private void OnDisable()
        {
            ResetCurrentTask();
        }

        protected virtual void FixedUpdate()
        {
            foreach (var action in _actions)
            {
                if (action == _currentAction)
                {
                    action.Power -= Time.fixedDeltaTime;
                    continue;
                }
                action.Power += Time.fixedDeltaTime / _actions.Count - 1;
            }
            if (_currentAction.Power < 0)
                _currentAction.Task.Stop();
        }

        #endregion Unity

        #region Protected

        protected abstract void CreateActions();

        protected void AddAction(AIAction aiAction)
        {
            _actions.Add(aiAction);
        }

        #endregion Protected

        #region Private

        private void RandomizeAction()
        {
            _currentAction = _actions.RandomElementByWeight(action => action.Power);
        }

        private void ResetCurrentTask()
        {
            _currentAction?.ResetTask(true);
            _currentAction?.TaskFinished.AddListener(RandomizeAction);
        }

        private void StartCurrentTask()
        {
            _currentAction?.StartTask();
        }

        #endregion Private
    }
}