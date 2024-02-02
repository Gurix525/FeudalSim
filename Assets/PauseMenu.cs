using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable()
    {
        AnalyticsBase.Send();
    }
}