using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [Header("Alpha Settings")]
    [SerializeField] private float activeAlpha = 1f;
    [SerializeField] private float inactiveAlpha = 0f;
    public RotatingSwitch rotatingSwitch;
    // public SpriteButton spriteButton;
    private SpriteRenderer spriteRenderer;
    private string imageName = "1";
    private Coroutine blinkCoroutine;
    private Coroutine longPressCoroutine;
    public static bool isActivMessageToAnotherButtons = false;
    public static event Action<bool> OnIndicatorStateChanged;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip signalClip;
    private AudioSource audioSource;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        SpriteLeftButton.OnButtonPressed += HandleOnButtonPressed;
        SpriteLeftButton.OnButtonReleased += HandleOnButtonReleased;
        SpriteRightButton.OnRightButtonPressed += HandleOnButtonRightPressed;
        rotatingSwitch.OnAngleChanged += HandleAngleChanged;
        OnIndicatorStateChanged += HandleIndicatorStateChanged;
    }


    private void HandleIndicatorStateChanged(bool newState)
    {
        if (!newState)
        {
            StopBlinkCoroutine();
            SetAlpha(activeAlpha);
        }
        else
        {
            if (blinkCoroutine == null)
            {
                SetAlpha(activeAlpha);
                blinkCoroutine = StartCoroutine(BlinkIndicators());
            }
        }
    }

    private void HandleOnButtonRightPressed(string buttonName)
    {
        if (!GlobalState.IsPower())
        {
            SetAlpha(inactiveAlpha);
            isActivMessageToAnotherButtons = false;
            // StopBlinkCoroutine();
            return;
        }

        if (GlobalState.IsActivBAvtBHand())
        {
            SetAlpha(activeAlpha);
            if (gameObject.name == "Window1")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + "88");
            if (gameObject.name == "Window2")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + "8");
            if (gameObject.name == "Window3")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + "08");
            isActivMessageToAnotherButtons = false;
            // StopBlinkCoroutine();
            return;
        }

        SetAlpha(inactiveAlpha);
        if (gameObject.name == "Window1")
            spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);
        if (gameObject.name == "Window2")
            spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);
        if (gameObject.name == "Window3")
            spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);

    }

    private void OnDisable()
    {
        SpriteLeftButton.OnButtonPressed -= HandleOnButtonPressed;
        SpriteLeftButton.OnButtonReleased -= HandleOnButtonReleased;
        SpriteRightButton.OnRightButtonPressed -= HandleOnButtonRightPressed;
        rotatingSwitch.OnAngleChanged -= HandleAngleChanged;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (longPressCoroutine != null)
        {
            StopLongPressCoroutine();
            longPressCoroutine = null;
        }
    }

    private void HandleOnButtonPressed(string name)
    {
        Debug.Log("GlobalState.IsPower: " + GlobalState.IsPower());
        if (!GlobalState.IsPower())
        {
            SetAlpha(inactiveAlpha);
            return;
        }

        if (name == SceneObjectNames.BUTTON_VK)
        {
            StopLongPressCoroutine();
            longPressCoroutine = StartCoroutine(WaitForLongPress());
            return;

            // if (!GlobalState.IsActivBAvtBHand())
            // {
            //     bool numberComparer = imageName == "1" || imageName == "2" || imageName == "3";
            //     if ((gameObject.name == "Window3" && numberComparer) || isActivMessageToAnotherButtons)
            //     {
            //         if (gameObject.name == "Window3" && numberComparer)
            //         {
            //             isActivMessageToAnotherButtons = true;
            //             OnIndicatorStateChanged?.Invoke(true);
            //         }
            //         if (blinkCoroutine == null)
            //         {
            //             SetAlpha(activeAlpha);
            //             blinkCoroutine = StartCoroutine(BlinkIndicators());
            //         }
            //     }
            // }
            // return;
        }

        Debug.Log("gameObject.name == ButtonNames.BUTTON_HAND " + gameObject.name == SceneObjectNames.BUTTON_HAND);
        if (name == SceneObjectNames.BUTTON_HAND)
        {
            SetAlpha(activeAlpha);
            return;
        }

    }

    private void HandleOnButtonReleased(string name)
    {
        if (!GlobalState.IsPower())
        {
            SetAlpha(inactiveAlpha);
            return;
        }

        if (name == SceneObjectNames.BUTTON_VK)
        {
            StopLongPressCoroutine();
            if (!GlobalState.IsActivBAvtBHand())
            {
                bool numberComparer = imageName == "1" || imageName == "2" || imageName == "3";
                if ((gameObject.name == "Window3" && numberComparer) || isActivMessageToAnotherButtons)
                {
                    StopBlinkCoroutine();
                    OnIndicatorStateChanged?.Invoke(false);
                    SetAlpha(activeAlpha);
                }
                return;
            }
        }

        if (name == SceneObjectNames.BUTTON_HAND)
        {
            SetAlpha(inactiveAlpha);
            return;
        }
    }

    private void HandleAngleChanged(string objectName, float rotationStep, float currentAngle)
    {
        float ratio = Mathf.Abs(currentAngle) / Mathf.Abs(rotationStep) + 1;
        imageName = Mathf.FloorToInt(ratio).ToString();

        Sprite newSprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning($"Спрайт с именем {imageName} не найден в папке Resources.");
        }
    }

    private IEnumerator WaitForLongPress()
    {
        yield return new WaitForSeconds(2f);
        // После 2 секунд проверяем условия и запускаем мигание, если они выполняются
        if (!GlobalState.IsActivBAvtBHand())
        {
            bool numberComparer = imageName == "1" || imageName == "2" || imageName == "3";
            if ((gameObject.name == "Window3" && numberComparer) || isActivMessageToAnotherButtons)
            {
                if (gameObject.name == "Window3" && numberComparer)
                {
                    isActivMessageToAnotherButtons = true;
                    OnIndicatorStateChanged?.Invoke(true);
                }
                if (blinkCoroutine == null)
                {
                    SetAlpha(activeAlpha);
                    blinkCoroutine = StartCoroutine(BlinkIndicators());
                }
            }
        }
    }

    private IEnumerator BlinkIndicators()
    {
        bool state = false;
        while (true)
        {
            bool numberComparer = imageName == "1" || imageName == "2" || imageName == "3";
            if ((gameObject.name == "Window3" && !numberComparer) || !isActivMessageToAnotherButtons)
            {
                SetAlpha(inactiveAlpha);
                isActivMessageToAnotherButtons = false;
                yield break;
            }

            state = !state;
            SetAlpha(state ? activeAlpha : inactiveAlpha);

            if (gameObject.name == "Window1")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);
            if (gameObject.name == "Window2")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);
            if (gameObject.name == "Window3")
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.IMAGE_NUMBERS_PATH + imageName);

            if (signalClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(signalClip);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void StopBlinkCoroutine()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private void StopLongPressCoroutine()
    {
        if (longPressCoroutine != null)
        {
            StopCoroutine(longPressCoroutine);
            longPressCoroutine = null;
        }
    }
}
