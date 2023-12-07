using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class AnalyticsInitializer : MonoBehaviour
{
    private async void Awake()
    {
        var options = new InitializationOptions();
        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);

        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "testParameter", 42 }
        };

        AnalyticsService.Instance.CustomData("testEvent", parameters);
    }
}