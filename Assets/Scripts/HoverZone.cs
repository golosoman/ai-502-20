using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HoverZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color hoverColor = new Color(1f, 1f, 0f, 0.3f);
    public Color normalColor = new Color(1f, 1f, 1f, 0f);

    private Image image;

    public static event Action<string> OnMouseEnterZone;
    public static event Action<string> OnMouseExitZone;
    public static event Action<string> OnMouseClickZone;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Компонент Image не найден на объекте " + gameObject.name);
        }
        else
        {
            image.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterZone?.Invoke(gameObject.name);
        if (image != null)
        {
            image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitZone?.Invoke(gameObject.name);
        if (image != null)
        {
            image.color = normalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnMouseClickZone?.Invoke(gameObject.name);
    }
}
