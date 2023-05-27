using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace AI
{
    public abstract class AIBehaviour : MonoBehaviour
    {
        #region Fields

        protected System.Random _random = new();

        private AIAction _currentAction;
        private List<AIAction> _actions = new();

        #endregion Fields

        #region Properties

        public Animal Animal { get; set; }
        public Agent Agent { get; set; }

        public Component Focus => Animal.Focus;

        #endregion Properties

        #region Public

        public void StopAction()
        {
            _currentAction?.StopTask();
        }

        #endregion Public

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
            StopAndDisableCurrentAction();
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

        protected void AddAction(Func<IEnumerator> getCoroutine)
        {
            _actions.Add(getCoroutine);
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

        private void StopAndDisableCurrentAction()
        {
            _currentAction?.StopTask();
            _currentAction?.TaskFinished.RemoveAllListeners();
        }

        #endregion Private
    }
}