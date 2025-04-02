using System;
using UnityEngine;

public class ActivePanelChecker : MonoBehaviour
{
    public static event Action<string> OnCheckActive;

    private void OnEnable()
    {
        OnCheckActive?.Invoke("Example message");
    }

    private void OnDisable()
    {
    }
}
