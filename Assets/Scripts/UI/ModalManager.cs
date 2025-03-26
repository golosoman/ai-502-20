using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public static ModalManager Instance;

    private Dictionary<string, GameObject> modalWindows = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (Transform child in transform)
        {
            modalWindows[child.gameObject.name] = child.gameObject;
            child.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        HoverZone.OnMouseClickZone += ShowModal;
    }

    private void OnDisable()
    {
        HoverZone.OnMouseClickZone -= ShowModal;
    }

    private void ShowModal(string zoneName)
    {
        CloseAllModals();

        string modalName = "Modal" + zoneName.Replace("Zone", "");

        if (modalWindows.TryGetValue(modalName, out GameObject modal))
        {
            modal.SetActive(true);
        }
        else
        {
            // Debug.LogWarning($"Модальное окно {modalName} не найдено!");
        }
    }

    public void CloseAllModals()
    {
        foreach (var modal in modalWindows.Values)
        {
            modal.SetActive(false);
        }
    }
}
