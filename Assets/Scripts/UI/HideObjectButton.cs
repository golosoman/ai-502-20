using UnityEngine;

public class HideObjectButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Объект, который нужно скрыть

    public void HideObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Не назначен объект для скрытия!");
        }
    }
}
