using System.Collections.Generic;
using UnityEngine;

public class ModalManager : MonoBehaviour
{
    public static ModalManager Instance;

    // Словарь для соответствия зон и модальных окон
    private Dictionary<string, GameObject> modalWindows = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Находим все модальные окна в сцене и скрываем их
        foreach (Transform child in transform)
        {
            modalWindows[child.gameObject.name] = child.gameObject;
            child.gameObject.SetActive(false); // Скрываем по умолчанию
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

    // Метод для открытия нужного модального окна
    private void ShowModal(string zoneName)
    {
        string modalName = "Modal" + zoneName.Replace("Zone", ""); // Преобразуем Zone1 → Modal1

        if (modalWindows.TryGetValue(modalName, out GameObject modal))
        {
            modal.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Модальное окно {modalName} не найдено!");
        }
    }

    // Метод для закрытия всех модальных окон
    public void CloseAllModals()
    {
        foreach (var modal in modalWindows.Values)
        {
            modal.SetActive(false);
        }
    }
}
