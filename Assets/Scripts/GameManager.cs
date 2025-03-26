using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<string, string> OnChangeImageIndicator;
    public static event Action<string, bool> OnChangeActiveIndicator;
    public static event Action<bool> OnPlaySound;
    public static event Action<bool> OnIndicatorStateChanged;
    private static List<string> windowsIndicator = new() { GlobalState.baseStateImage, GlobalState.baseStateImage, GlobalState.baseStateImage };
    private Coroutine blinkCoroutine;
    private Coroutine longPressCoroutine;

    private void OnEnable()
    {
        TMPButtonColorToggle.OnButtonClicked += HandleSpriteClick;
        SpriteRightButton.OnRightButtonPressed += HandleRightButtonPress;
        RotateOnClick.OnButtonToggled += HandleBortButtonToggled;
        SpriteLeftButton.OnLeftButtonPressed += HandleOnLeftButtonPressed;
        SpriteLeftButton.OnLeftButtonReleased += HandleOnLeftButtonReleased;
        RotatingSwitch.OnSwitched += HandleOnSwitcher;
        OnIndicatorStateChanged += HandleIndicatorStateChanged;
    }

    private void HandleOnSwitcher(string name, float rotationStep, float currentAngle)
    {
        float ratio = Mathf.Abs(currentAngle) / Mathf.Abs(rotationStep) + 1;
        string imageName = Mathf.FloorToInt(ratio).ToString();

        if (name == SceneObjectNames.SWITCH_FIRST)
        {
            windowsIndicator[0] = imageName;
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_FIRST, imageName);
        }

        if (name == SceneObjectNames.SWITCH_SECOND)
        {
            windowsIndicator[1] = imageName;
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_SECOND, imageName);
        }

        if (name == SceneObjectNames.SWITCH_THIRD)
        {
            windowsIndicator[2] = imageName;
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_THIRD, imageName);
        }
    }

    private void HandleOnLeftButtonPressed(string name)
    {
        if (!GlobalState.IsPower())
        {
            OnChangeActiveIndicator?.Invoke("ExapleWindow", false);
            return;
        }

        if (name == SceneObjectNames.BUTTON_HAND)
        {
            OnChangeActiveIndicator?.Invoke("ExapleWindow", true);
            return;
        }

        if (GlobalState.IsActivPowerAndVORK())
        {
            if (name == SceneObjectNames.BUTTON_VK)
            {
                StopLongPressCoroutine();
                longPressCoroutine = StartCoroutine(WaitForLongPress());
                return;
            }
        }
    }

    private void HandleOnLeftButtonReleased(string name)
    {
        if (!GlobalState.IsPower())
        {
            OnChangeActiveIndicator?.Invoke("ExapleWindow", false);
            return;
        }

        if (name == SceneObjectNames.BUTTON_HAND)
        {
            OnChangeActiveIndicator?.Invoke("ExapleWindow", false);
            return;
        }

        if (name == SceneObjectNames.BUTTON_VK)
        {
            StopLongPressCoroutine();
            if (!GlobalState.isActivButtonHand && !GlobalState.isActivButtonAvt && GlobalState.isActivORK)
            {
                if (isActivTask1() || isActivTask2())
                {
                    StopBlinkCoroutine();
                    OnIndicatorStateChanged?.Invoke(false);
                }

                if (!isActivTask1() && !isActivTask2())
                {
                    StopBlinkCoroutine();
                    OnChangeActiveIndicator?.Invoke("ExapleWindow", false);
                }

            }
        }
    }

    private void OnDisable()
    {
        SpriteSwitcher.OnSpriteClicked -= HandleSpriteClick;
        SpriteRightButton.OnRightButtonPressed -= HandleRightButtonPress;
        RotateOnClick.OnButtonToggled -= HandleBortButtonToggled;
        SpriteLeftButton.OnLeftButtonPressed -= HandleOnLeftButtonPressed;
        SpriteLeftButton.OnLeftButtonReleased -= HandleOnLeftButtonReleased;
        RotatingSwitch.OnSwitched -= HandleOnSwitcher;
        OnIndicatorStateChanged -= HandleIndicatorStateChanged;
    }

    private void HandleBortButtonToggled(string buttonName)
    {
        if (buttonName == SceneObjectNames.BUTTON_ACCUMULATOR_BORT)
        {
            GlobalState.isActivButtonBortAcum = !GlobalState.isActivButtonBortAcum;
        }

        if (buttonName == SceneObjectNames.BUTTON_SYSTEM_BORT)
        {
            GlobalState.isActivButtonBortSyst = !GlobalState.isActivButtonBortSyst;
        }
    }

    private void HandleRightButtonPress(string buttonName)
    {
        if (buttonName == SceneObjectNames.BUTTON_HAND)
        {
            GlobalState.isActivButtonHand = !GlobalState.isActivButtonHand;
        }

        if (buttonName == SceneObjectNames.BUTTON_AVT)
        {
            GlobalState.isActivButtonAvt = !GlobalState.isActivButtonAvt;
        }

        if (buttonName == SceneObjectNames.BUTTON_VK)
        {
            GlobalState.isActivButtonVK = !GlobalState.isActivButtonVK;
        }

        if (!GlobalState.IsPower())
        {
            return;
        }

        // TODO В условии есть багуля
        if (GlobalState.IsActivBAvtBHandWithoutORK())
        {
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_FIRST, "88");
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_SECOND, "8");
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_THIRD, "08");
            OnChangeActiveIndicator?.Invoke("ExampleWindows", true);
            return;
        }

        OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_FIRST, windowsIndicator[0]);
        OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_SECOND, windowsIndicator[1]);
        OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_THIRD, windowsIndicator[2]);
        OnChangeActiveIndicator?.Invoke("ExampleWindows", false);
    }

    private void HandleSpriteClick(string objectName)
    {
        if (objectName == SceneObjectNames.B_HEAD_SET)
        {
            GlobalState.isActivORK = !GlobalState.isActivORK;
        }
    }

    private bool isActivTask1()
    {
        bool numberComparerIndicator31 = new[] { SceneObjectNames.ONE, SceneObjectNames.TWO, SceneObjectNames.THREE }.Contains(windowsIndicator[2]);
        bool numberComparerIndicator32 = numberComparerIndicator31 || windowsIndicator[2] == SceneObjectNames.FOUR;
        bool numberComparerIndicator21 = new[] {
            SceneObjectNames.FOUR, SceneObjectNames.FIVE, SceneObjectNames.SIX, SceneObjectNames.SEVEN, SceneObjectNames.EIGHT,
            SceneObjectNames.NINE, SceneObjectNames.TEN, SceneObjectNames.ELEVEN, SceneObjectNames.TWELVE, SceneObjectNames.THIRTEEN,

            SceneObjectNames.FOURTEEN, SceneObjectNames.FIFTEEN }.Contains(windowsIndicator[1]);
        return numberComparerIndicator32 && numberComparerIndicator21;
    }

    private bool isActivTask2()
    {
        bool numberComparerIndicator31 = new[] { SceneObjectNames.ONE, SceneObjectNames.TWO, SceneObjectNames.THREE }.Contains(windowsIndicator[2]);
        return numberComparerIndicator31;
    }

    private IEnumerator WaitForLongPress()
    {
        yield return new WaitForSeconds(2f);
        if (!GlobalState.IsActivBAvtBHand())
        {
            bool numberComparerIndicator31 = new[] { SceneObjectNames.ONE, SceneObjectNames.TWO, SceneObjectNames.THREE }.Contains(windowsIndicator[2]);
            bool numberComparerIndicator32 = numberComparerIndicator31 || windowsIndicator[2] == SceneObjectNames.FOUR;
            bool numberComparerIndicator21 = new[] {
                SceneObjectNames.FOUR, SceneObjectNames.FIVE, SceneObjectNames.SIX, SceneObjectNames.SEVEN, SceneObjectNames.EIGHT,
                SceneObjectNames.NINE, SceneObjectNames.TEN, SceneObjectNames.ELEVEN, SceneObjectNames.TWELVE, SceneObjectNames.THIRTEEN,

                SceneObjectNames.FOURTEEN, SceneObjectNames.FIFTEEN }.Contains(windowsIndicator[1]);
            bool flag = false;

            if (isActivTask1())
            {
                if (blinkCoroutine == null)
                {
                    flag = true;
                    OnChangeActiveIndicator?.Invoke("ExampleWindows", true);
                    blinkCoroutine = StartCoroutine(BlinkIndicators());
                }
            }
            else if (!flag && isActivTask2())
            {
                if (blinkCoroutine == null)
                {
                    OnChangeActiveIndicator?.Invoke("ExampleWindows", true);
                    blinkCoroutine = StartCoroutine(BlinkIndicators());
                }
            }
            else
            {
                OnChangeActiveIndicator?.Invoke("ExampleWindows", false);
            }
        }
    }

    private void HandleIndicatorStateChanged(bool newState)
    {
        if (!newState)
        {
            StopBlinkCoroutine();
            OnChangeActiveIndicator?.Invoke("ExampleWindows", true);
        }
        else
        {
            if (blinkCoroutine == null)
            {
                OnChangeActiveIndicator?.Invoke("ExampleWindows", true);
                blinkCoroutine = StartCoroutine(BlinkIndicators());
            }
        }
    }


    private IEnumerator BlinkIndicators()
    {
        bool state = false;
        while (true)
        {
            state = !state;
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_FIRST, windowsIndicator[0]);
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_SECOND, windowsIndicator[1]);
            OnChangeImageIndicator?.Invoke(SceneObjectNames.WINDOW_THIRD, windowsIndicator[2]);

            OnChangeActiveIndicator?.Invoke("ExampleWindows", state ? true : false);

            if (GlobalState.isActivORK)
            {
                OnPlaySound?.Invoke(true);
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
