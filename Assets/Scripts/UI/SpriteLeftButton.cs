using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SpriteLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Visual Settings")]
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private GameObject borderHighlight;
    public static event Action<string> OnLeftButtonPressed;
    public static event Action<string> OnLeftButtonReleased;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isActive;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        isActive = false;
        UpdateAppearance();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;
        transform.localScale = originalScale * pressedScale;

        isActive = true;
        UpdateAppearance();

        OnLeftButtonPressed?.Invoke(gameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;

        isActive = false;
        UpdateAppearance();

        OnLeftButtonReleased?.Invoke(gameObject.name);
    }

    private void UpdateAppearance()
    {
        if (borderHighlight)
            borderHighlight.SetActive(isActive);
    }

    private void OnMouseDown() => OnPointerDown(null);
    private void OnMouseUp() => OnPointerUp(null);
}
