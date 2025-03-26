using UnityEngine;
using System;

public class SpriteSwitcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSpriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private int currentSpriteIndex = 0;
    public static event Action<string> OnSpriteClicked;
    private Color originalColor;
    private Color toggledColor = Color.green;
    private bool isToggled = false;

    private void Start()
    {
        if (targetSpriteRenderer == null)
        {
            targetSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (sprites.Length > 0)
        {
            targetSpriteRenderer.sprite = sprites[currentSpriteIndex];
        }

        originalColor = targetSpriteRenderer.color;
    }

    private void OnMouseDown()
    {
        NextSprite();
        ChangeState();
        OnSpriteClicked?.Invoke(gameObject.name);
    }

    public void ChangeState()
    {
        isToggled = !isToggled;
        targetSpriteRenderer.color = isToggled ? toggledColor : originalColor;

        OnSpriteClicked?.Invoke(gameObject.name);
    }

    public void SetSprite(int index)
    {
        if (index >= 0 && index < sprites.Length)
        {
            currentSpriteIndex = index;
            targetSpriteRenderer.sprite = sprites[currentSpriteIndex];
        }
        else
        {
            // Debug.LogWarning("Индекс выходит за пределы массива спрайтов.");
        }
    }

    public void NextSprite()
    {
        if (sprites.Length > 0)
        {
            currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Length;
            targetSpriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }

    public void PreviousSprite()
    {
        if (sprites.Length > 0)
        {
            currentSpriteIndex--;
            if (currentSpriteIndex < 0)
            {
                currentSpriteIndex = sprites.Length - 1;
            }
            targetSpriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
}
