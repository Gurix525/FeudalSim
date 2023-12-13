using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    public static void SendEvent(string eventName, params EventParameter[] parameters)
    {
        Dictionary<string, object> dictionary = new();
        foreach (EventParameter parameter in parameters)
        {
            dictionary[parameter.Name] = parameter.Value;
        }

        AnalyticsService.Instance.CustomData(eventName, dictionary);
    }

    private async void Awake()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);
        AnalyticsService.Instance.StartDataCollection();
    }
}
