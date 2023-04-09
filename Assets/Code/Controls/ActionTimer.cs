using System;
using System.Threading;
using System.Threading.Tasks;
using Input;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public static class ActionTimer
    {
        private static CancellationTokenSource _tokenSource = new();

        public static float CompletionRatio { get; private set; }

        public static async Task Start(Action action, float requiredTime)
        {
            _tokenSource = new();
            PlayerController.MainUse.AddListener(ActionType.Canceled, CancelTask);
            var task = RunTimer(requiredTime);
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    PlayerController.MainUse.RemoveListener(ActionType.Canceled, CancelTask);
                    return;
                }
                if (task.IsCompleted)
                {
                    action();
                    PlayerController.MainUse.RemoveListener(ActionType.Canceled, CancelTask);
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