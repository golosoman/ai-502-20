using UnityEngine;

public class HideObjectButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    public void HideObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        else
        {
            // Debug.LogWarning("Не назначен объект для скрытия!");
        }
    }
}
