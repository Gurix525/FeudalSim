using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Canvases : MonoBehaviour
{
    public event EventHandler<string> CommandPassed;

    private string _lastCommand = "Clear";

    private void OnBuilding(InputValue value)
    {
        SendCommand("Building");
    }

    private void OnChange(InputValue value)
    {
        SendCommand("Combat");
    }

    private void OnTab(InputValue value)
    {
        SendCommand("Inventory");
    }

    private void OnMap(InputValue value)
    {
        SendCommand("Map");
    }

    private void SendCommand(string command)
    {
        if (_lastCommand == command)
        {
            CommandPassed?.Invoke(this, "Clear");
            _lastCommand = "Clear";
        }
        else
        {
            CommandPassed?.Invoke(this, command);
            _lastCommand = command;// == "Clear" ? "Inventory" : command;
        }
    }
}