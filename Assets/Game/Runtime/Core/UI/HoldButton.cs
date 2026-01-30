using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    public bool IsHolding { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHolding = false;
    }

    // 手指/鼠标移出按钮时也要停
    public void OnPointerExit(PointerEventData eventData)
    {
        IsHolding = false;
    }
}