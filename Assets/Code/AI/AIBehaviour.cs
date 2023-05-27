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
            enabled = false;
        }

        private void OnEnable()
        {
            RandomizeAction();
        }

        private void OnDisable()
        {
            ResetCurrentAction();
        }

        protected virtual void FixedUpdate()
        {
            foreach (var action in _actions)
            {
                if (action == _currentAction)
                {
                    action.Power -= Time.fixedDeltaTime;
                    action.Power = action.Power < 0 ? 0 : action.Power;
                    continue;
                }
                action.Power += Time.fixedDeltaTime / (_actions.Count - 1);
            }
            if (_currentAction?.Power <= 0)
                _currentAction?.Task?.Stop();
        }

        #endregion Unity

        #region Protected

        protected virtual void CreateActions()
        {
        }

        protected void AddAction(AIAction aiAction)
        {
            _actions.Add(aiAction);
        }

        #endregion Protected

        #region Private

        private void RandomizeAction()
        {
            _currentAction = _actions.RandomElementByWeight(action => action.Power);
            ResetCurrentAction();
            StartCurrentAction();
        }

        private void ResetCurrentAction()
        {
            _currentAction?.ResetTask(true);
            _currentAction?.TaskFinished.AddListener(RandomizeAction);
        }

        private void StartCurrentAction()
        {
            _currentAction?.StartTask();
        }

        #endregion Private
    }
}