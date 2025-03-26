using UnityEngine;
using System;

public class RotateOnClick : MonoBehaviour
{
    public static event Action<string> OnButtonToggled;
    private bool isRotated = false;

    private void OnMouseDown()
    {
        transform.Rotate(0, 0, 180);
        isRotated = !isRotated;
        OnButtonToggled?.Invoke(gameObject.name);
    }
}
