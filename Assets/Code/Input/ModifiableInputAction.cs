using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Input
{
    public class ModifiableInputAction
    {
        #region Fields

        private List<Action<CallbackContext>> _startedCallbacks = new();
        private List<Action<CallbackContext>> _performedCallbacks = new();
        private List<Action<CallbackContext>> _canceledCallbacks = new();

        #endregion Fields

        #region Properties

        public InputAction Action { get; set; }

        #endregion Properties

        #region Public

        public void ClearAllEvents()
        {
            PlayerController.Initialize();
            ClearStartedEvent();
            ClearPerformedEvent();
            ClearCanceledEvent();
        }

        public void ClearStartedEvent()
        {
            PlayerController.Initialize();
            _startedCallbacks.ForEach(callback => Action.started -= callback);
            _startedCallbacks.Clear();
        }

        public void ClearPerformedEvent()
        {
            PlayerController.Initialize();
            _performedCallbacks.ForEach(callback => Action.performed -= callback);
            _performedCallbacks.Clear();
        }

        public void ClearCanceledEvent()
        {
            PlayerController.Initialize();
            _canceledCallbacks.ForEach(callback => Action.canceled -= callback);
            _canceledCallbacks.Clear();
        }

        public void AddListener(ActionType actionType, Action<CallbackContext> callback)
        {
            PlayerController.Initialize();
            switch (actionType)
            {
                case ActionType.Started:
                    _startedCallbacks.Add(callback);
                    AddStartedCallback(callback);
                    break;

                case ActionType.Performed:
                    _performedCallbacks.Add(callback);
                    AddPerformedCallback(callback);
                    break;

                case ActionType.Canceled:
                    _canceledCallbacks.Add(callback);
                    AddCanceledCallback(callback);
                    break;
            }
        }

        public void RemoveListener(ActionType actionType, Action<CallbackContext> callback)
        {
            switch (actionType)
            {
                case ActionType.Started:
                    _startedCallbacks.Remove(callback);
                    RemoveStartedCallback(callback);
                    break;

                case ActionType.Performed:
                    _performedCallbacks.Remove(callback);
                    RemovePerformedCallback(callback);
                    break;

                case ActionType.Canceled:
                    _canceledCallbacks.Remove(callback);
                    RemovePerformedCallback(callback);
                    break;
            }
        }

        public TValue ReadValue<TValue>()
            where TValue : struct
        {
            PlayerController.Initialize();
            return Action.ReadValue<TValue>();
        }

        public bool IsPressed()
        {
            PlayerController.Initialize();
            return Action.IsPressed();
        }

        #endregion Public

        #region Private

        private void AddStartedCallback(Action<CallbackContext> callback)
        {
            Action.started += callback;
        }

        private void AddPerformedCallback(Action<CallbackContext> callback)
        {
            Action.performed += callback;
        }

        private void AddCanceledCallback(Action<CallbackContext> callback)
        {
            Action.canceled += callback;
        }

        private void RemoveStartedCallback(Action<CallbackContext> callback)
        {
            Action.started -= callback;
        }

        private void RemovePerformedCallback(Action<CallbackContext> callback)
        {
            Action.performed -= callback;
        }

        private void RemoveCanceledCallback(Action<CallbackContext> callback)
        {
            Action.canceled -= callback;
        }

        #endregion Private
    }
}