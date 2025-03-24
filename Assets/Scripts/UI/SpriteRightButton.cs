using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SpriteRightButton : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private GameObject borderObject;

    public delegate void ButtonEventHandler(string buttonName);

    public static event ButtonEventHandler OnRightButtonPressed;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;

    // Флаг, указывающий, активен ли объект
    private bool isActive = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        UpdateAppearance();
    }

    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     print(eventData.button);
    //     if (eventData.button == PointerEventData.InputButton.Right)
    //     {
    //         // Визуальный эффект нажатия
    //         spriteRenderer.color = pressedColor;
    //         transform.localScale = originalScale * pressedScale;
    //         ToggleActiveState();

    //         // Вызываем событие нажатия
    //         OnButtonPressed?.Invoke(gameObject.name);
    //     }
    // }

    // private void OnMouseOver()
    // {
    //     if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
    //     {
    //         UpdateAppearance();
    //     }
    // }

    private void ChangeStateButton()
    {
        if (gameObject.name == ButtonNames.BUTTON_HAND)
            GlobalState.isActivButtonHand = !GlobalState.isActivButtonHand;
        if (gameObject.name == ButtonNames.BUTTON_AVT)
            GlobalState.isActivButtonAvt = !GlobalState.isActivButtonAvt;
        if (gameObject.name == ButtonNames.BUTTON_VK)
            GlobalState.isActivButtonVK = !GlobalState.isActivButtonVK;
        // TODO
    }

    void Update()
    {
        // Проверяем, был ли выполнен правый клик мыши
        if (Input.GetMouseButtonDown(1)) // 1 - это правый клик мыши
        {
            // Создаем луч от камеры к позиции мыши
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Проверяем, попал ли луч в текущий объект
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                // Визуальный эффект нажатия
                spriteRenderer.color = pressedColor;
                transform.localScale = originalScale * pressedScale;
                ChangeStateButton();
                ToggleActiveState();

                // Вызываем событие нажатия
                OnRightButtonPressed?.Invoke(gameObject.name);
            }
        }
    }

    private void UpdateAppearance()
    {
        if (borderObject) borderObject.SetActive(isActive);
    }

    // Метод для переключения состояния объекта
    private void ToggleActiveState()
    {
        isActive = !isActive; // Инвертируем состояние

        UpdateAppearance();

        // Можно добавить дополнительную логику, например, изменение цвета спрайта
        Debug.Log(isActive ? "Объект активен" : "Объект неактивен");
    }


    // private void OnMouseDown() => OnPointerDown(GetMouseEventData());
    // private void OnMouseDown() => OnPointerDown();
    // private PointerEventData GetMouseEventData()
    // {
    //     return new PointerEventData(EventSystem.current)
    //     {
    //         button = Input.GetMouseButton(0) ?
    //             PointerEventData.InputButton.Left :
    //             PointerEventData.InputButton.Right
    //     };
    // }
}
