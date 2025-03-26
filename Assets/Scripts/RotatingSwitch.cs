using UnityEngine;
using System;

public class RotatingSwitch : MonoBehaviour
{
    public float rotationStep = 18f;
    public float maxAngle = 18f;
    private float currentAngle = 0f;
    private bool isRotatingClockwise = true;
    public event Action<string, float, float> OnAngleChanged;
    public static event Action<string, float, float> OnSwitched;

    private void Start()
    {
        currentAngle = transform.localRotation.eulerAngles.z;
    }

    private void OnMouseDown()
    {
        RotateSwitch();
    }

    private void RotateSwitch()
    {
        if (isRotatingClockwise)
        {
            currentAngle -= rotationStep;
            if (currentAngle == maxAngle)
            {
                currentAngle = maxAngle;
                isRotatingClockwise = false;
            }
        }
        else
        {
            currentAngle += rotationStep;
            if (currentAngle == 0)
            {
                currentAngle = 0;
                isRotatingClockwise = true;
            }
        }
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        OnSwitched?.Invoke(gameObject.name, Mathf.Abs(rotationStep), Mathf.Abs(currentAngle));
        OnAngleChanged?.Invoke(gameObject.name, Mathf.Abs(rotationStep), Mathf.Abs(currentAngle));
    }

}
