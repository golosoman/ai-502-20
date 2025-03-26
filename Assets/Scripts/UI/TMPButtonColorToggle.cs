using System;
using UnityEngine;
using UnityEngine.UI;

public class TMPButtonColorToggle : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    private Color originalColor;
    private Color targetColor = Color.green;
    private bool isToggled = false;
    public static event Action<string> OnButtonClicked;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }

        if (button != null)
        {
            button.onClick.AddListener(ToggleColor);
        }
    }

    void ToggleColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isToggled ? originalColor : targetColor;
            isToggled = !isToggled;
            OnButtonClicked?.Invoke(gameObject.name);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}