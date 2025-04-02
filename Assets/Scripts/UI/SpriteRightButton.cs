using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SpriteRightButton : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private GameObject borderObject;
    public static event Action<string> OnRightButtonPressed;


    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private bool isActive = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        UpdateAppearance();
    }

    private void OnEnable()
    {
        GameManager.OnResetRightButton += HandleResetRightButton;
    }

    private void OnDisable()
    {
        GameManager.OnResetRightButton -= HandleResetRightButton;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                ToggleActiveState();
                OnRightButtonPressed?.Invoke(gameObject.name);
            }
        }
    }

    private void ToggleActiveState()
    {
        isActive = !isActive;

        if (isActive)
        {
            spriteRenderer.color = pressedColor;
            transform.localScale = originalScale * pressedScale;
        }
        else
        {
            spriteRenderer.color = originalColor;
            transform.localScale = originalScale;
        }

        UpdateAppearance();
    }

    private void HandleResetRightButton(bool resetValue)
    {
        isActive = false;
        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (borderObject)
            borderObject.SetActive(isActive);
    }
}
