using System;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [Header("Alpha Settings")]
    [SerializeField] private float activeAlpha = 1f;
    [SerializeField] private float inactiveAlpha = 0f;
    private SpriteRenderer spriteRenderer;
    private string imageName = GlobalState.baseStateImage;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip signalClip;
    private static AudioSource audioSource;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);
        SetAlpha(inactiveAlpha);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = Mathf.Clamp01(alpha);
        spriteRenderer.color = color;
    }

    private void OnEnable()
    {
        GameManager.OnChangeActiveIndicator += HandleChangeActiveIndicator;
        GameManager.OnChangeImageIndicator += HandleChangeImageIndicator;
        GameManager.OnPlaySound += HandlePlaySound;
    }

    private void HandlePlaySound(bool isPlay)
    {
        if (signalClip != null && audioSource != null)
        {
            if (isPlay)
            {
                audioSource.PlayOneShot(signalClip);
            }
        }
    }

    private void HandleChangeActiveIndicator(string name, bool isActive)
    {
        if (isActive)
        {
            SetAlpha(activeAlpha);
        }

        if (!isActive)
        {
            SetAlpha(inactiveAlpha);
        }
    }

    private void HandleChangeImageIndicator(string windowName, string image)
    {
        if (gameObject.name == windowName)
        {
            imageName = image;
            Sprite newSprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);

            if (newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
            else
            {
                // Debug.LogWarning($"Спрайт с именем {imageName} не найден в папке Resources.");
            }
        }
    }

    private void OnDisable()
    {
        GameManager.OnChangeActiveIndicator -= HandleChangeActiveIndicator;
        GameManager.OnChangeImageIndicator -= HandleChangeImageIndicator;
        GameManager.OnPlaySound -= HandlePlaySound;
    }
}
