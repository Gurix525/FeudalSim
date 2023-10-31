using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Canvases : MonoBehaviour
{
    public event EventHandler<string> CommandPassed;

    private string _lastCommand = "Inventory";

    private void Awake()
    {
        CommandPassed += Canvases_CommandPassed;
    }

    private void OnBuilding(InputValue value)
    {
        CommandPassed?.Invoke(this, "Building");
    }

    private void OnChange(InputValue value)
    {
        CommandPassed?.Invoke(this, "Combat");
    }

    private void OnTab(InputValue value)
    {
        if (_lastCommand == "Inventory")
            CommandPassed?.Invoke(this, "Inventory");
        else
            CommandPassed?.Invoke(this, "Clear");
    }

    private void Canvases_CommandPassed(object sender, string e)
    {
        _lastCommand = e == "Clear" ? "Inventory" : e;
    }
}