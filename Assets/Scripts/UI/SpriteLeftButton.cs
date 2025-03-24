using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SpriteLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void ButtonEventHandler(string buttonName);
    public static event ButtonEventHandler OnButtonPressed;
    public static event ButtonEventHandler OnButtonReleased;

    public UnityEvent<bool> OnToggle;

    [Header("Visual Settings")]
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private GameObject borderHighlight;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    // Локальный флаг, отражающий состояние кнопки
    private bool isActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        isActive = false; // Изначально кнопка неактивна
        UpdateAppearance();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Визуальные эффекты при нажатии
        spriteRenderer.color = pressedColor;
        transform.localScale = originalScale * pressedScale;

        // Устанавливаем флаг в true при нажатии
        isActive = true;
        UpdateAppearance();

        // Вызываем событие нажатия
        OnButtonPressed?.Invoke(gameObject.name);
        OnToggle?.Invoke(isActive);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Возвращаем исходный вид
        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;

        // При отпускании устанавливаем флаг в false
        isActive = false;
        UpdateAppearance();

        // Вызываем событие отжатия
        OnButtonReleased?.Invoke(gameObject.name);
        OnToggle?.Invoke(isActive);
    }

    private void UpdateAppearance()
    {
        // Задний объект активен, когда кнопка активна
        if (borderHighlight)
            borderHighlight.SetActive(isActive);
    }

    private void OnMouseDown() => OnPointerDown(null);
    private void OnMouseUp() => OnPointerUp(null);
}
