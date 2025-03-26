using UnityEngine;
using System;

public class HoverZone : MonoBehaviour
{
    public Color hoverColor = new Color(1f, 1f, 0f, 0.3f);
    public Color normalColor = new Color(1f, 1f, 1f, 0f);

    private SpriteRenderer spriteRenderer;

    public static event Action<string> OnMouseEnterZone;
    public static event Action<string> OnMouseExitZone;
    public static event Action<string> OnMouseClickZone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            Rect rect = new Rect(0, 0, 1, 1);
            spriteRenderer.sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
        }
        spriteRenderer.color = normalColor;
    }

    private void OnMouseEnter()
    {
        OnMouseEnterZone?.Invoke(gameObject.name);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        OnMouseExitZone?.Invoke(gameObject.name);
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
    }

    private void OnMouseDown()
    {
        OnMouseClickZone?.Invoke(gameObject.name);
    }
}
