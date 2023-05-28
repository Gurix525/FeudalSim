using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

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
        public UnityEvent StateUpdated { get; } = new();
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

        protected void OnEnable()
        {
            RandomizeAction();
            DuringEnable();
        }

        protected virtual void OnDisable()
        {
            StopAndDisableCurrentAction();
            StateUpdated.RemoveAllListeners();
        }

        #endregion Unity

        #region Protected

        protected abstract void CreateActions();

        protected virtual void DuringEnable()
        { }

        protected void AddAction(AIAction aiAction)
        {
            _actions.Add(aiAction);
        }

        protected void AddAction(Func<IEnumerator> getCoroutine)
        {
            _actions.Add(getCoroutine);
        }

        protected void SetSpeed(MoveSpeedType type)
        {
            Animal.MoveSpeed = type;
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