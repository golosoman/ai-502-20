using UnityEngine;

public class HoverZone : MonoBehaviour
{
    // Делегаты для событий
    public delegate void HoverEventHandler(string objectName);

    // События для входа, выхода и клика в зоне
    public static event HoverEventHandler OnMouseEnterZone;
    public static event HoverEventHandler OnMouseExitZone;
    public static event HoverEventHandler OnMouseClickZone;

    private void OnMouseEnter()
    {
        // Debug.Log($"Вы в зоне: {gameObject.name}");
        OnMouseEnterZone?.Invoke(gameObject.name);
    }

    private void OnMouseExit()
    {
        // Debug.Log($"Вы вышли из зоны: {gameObject.name}");
        OnMouseExitZone?.Invoke(gameObject.name);
    }

    private void OnMouseDown()
    {
        // Debug.Log($"Клик по зоне: {gameObject.name}");
        OnMouseClickZone?.Invoke(gameObject.name);
    }
}
