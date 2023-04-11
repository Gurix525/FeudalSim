using System;
using System.Threading;
using System.Threading.Tasks;
using Input;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public static class ActionTimer
    {
        private static CancellationTokenSource _tokenSource = new();

        public static UnityEvent<float> TimerSet { get; } = new();

        public static async Task Start(Action action, float requiredTime)
        {
            TimerSet.Invoke(requiredTime);
            _tokenSource = new();
            PlayerController.MainUse.AddListener(ActionType.Canceled, CancelTask);
            PlayerController.MainQuickMenu.AddListener(ActionType.Started, CancelTask);
            PlayerController.MainChange.AddListener(ActionType.Started, CancelTask);
            var task = RunTimer(requiredTime);
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    PlayerController.MainUse.RemoveListener(ActionType.Canceled, CancelTask);
                    PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, CancelTask);
                    PlayerController.MainChange.RemoveListener(ActionType.Started, CancelTask);
                    TimerSet.Invoke(0F);
                    return;
                }
                if (task.IsCompleted)
                {
                    action();
                    PlayerController.MainUse.RemoveListener(ActionType.Canceled, CancelTask);
                    PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, CancelTask);
                    PlayerController.MainChange.RemoveListener(ActionType.Started, CancelTask);
                    TimerSet.Invoke(0F);
                    return;
                }
                await Task.Yield();
            }
        }

        private static void CancelTask(CallbackContext context)
        {
            _tokenSource.Cancel();
            PlayerController.MainUse.RemoveListener(ActionType.Canceled, CancelTask);
        }

        private static async Task RunTimer(float requiredTime)
        {
            await Task.Delay((int)(requiredTime * 1000));
        }
    }
}