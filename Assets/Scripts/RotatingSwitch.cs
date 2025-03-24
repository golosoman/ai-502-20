using UnityEngine;

public class RotatingSwitch : MonoBehaviour
{
    public float rotationStep = 18f; // Шаг вращения
    public float maxAngle = 18f;    // Максимальный угол вращения
    private float currentAngle = 0f; // Текущий угол

    private bool isRotatingClockwise = true; // Направление вращения


    // Делегат для события
    public delegate void AngleChangedHandler(string objectName, float maxAngle, float currentAngle);
    // Событие, которое будет генерироваться при изменении угла
    public event AngleChangedHandler OnAngleChanged;

    private void Start()
    {
        // Устанавливаем начальное положение
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
            currentAngle -= rotationStep; // По часовой стрелке уменьшаем угол
            if (currentAngle == maxAngle)
            {
                currentAngle = maxAngle; // Ограничиваем угол по часовой стрелке
                isRotatingClockwise = false; // Меняем направление
            }
        }
        else
        {
            currentAngle += rotationStep; // Против часовой стрелки увеличиваем угол
            if (currentAngle == 0)
            {
                currentAngle = 0; // Ограничиваем угол против часовой стрелки
                isRotatingClockwise = true; // Меняем направление
            }
        }

        // Применяем новое вращение (в локальной системе координат)
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

        // Генерируем событие
        OnAngleChanged?.Invoke(gameObject.name, Mathf.Abs(rotationStep), Mathf.Abs(currentAngle)); // Вызываем событие с параметрами
    }

}
