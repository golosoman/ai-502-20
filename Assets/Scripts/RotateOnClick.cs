using UnityEngine;

public class RotateOnClick : MonoBehaviour
{

    private bool isRotated = false;

    private void OnMouseDown()
    {
        // Поворачиваем объект на 180 градусов
        transform.Rotate(0, 0, 180);
        isRotated = !isRotated;

        // Дополнительная логика в зависимости от имени объекта
        if (gameObject.name == ButtonNames.BUTTON_ACCUMULATOR_BORT)
        {
            FeatureForBAkumBort();
        }
        else if (gameObject.name == ButtonNames.BUTTON_SYSTEM_BORT)
        {
            FeatureForBSystBort();
        }

        // Вызываем событие с текущими состояниями
        // OnButtonStateChanged?.Invoke(GlobalState.isActivButtonBortAcum, GlobalState.isActivButtonBortSyst);
    }

    private void FeatureForBAkumBort()
    {
        GlobalState.isActivButtonBortAcum = !GlobalState.isActivButtonBortAcum;
        Debug.Log("Дополнительная фича для BAkumBort!" + GlobalState.isActivButtonBortAcum);
    }

    private void FeatureForBSystBort()
    {
        GlobalState.isActivButtonBortSyst = !GlobalState.isActivButtonBortSyst;
        Debug.Log("Дополнительная фича для BSystemBort!" + GlobalState.isActivButtonBortSyst);
    }
}
