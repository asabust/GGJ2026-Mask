using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Config")] public float snapDistance = 40f;
    public Vector2 correctPosition;
    public bool isPlaced;

    [HideInInspector] public RectTransform rect;
    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public JigsawPuzzlePanel panel;

    private Vector2 dragOffset;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 鼠标按下，记录偏移，避免“跳”
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position,
            eventData.pressEventCamera, out dragOffset
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling(); // 拖到最上层
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform,
            eventData.position, eventData.pressEventCamera, out var localPoint
        );

        rect.anchoredPosition = localPoint - dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = true;

        // 判断是否接近正确位置
        var distance = Vector2.Distance(rect.anchoredPosition, correctPosition);

        if (distance <= snapDistance) SnapToCorrectPosition();
    }

    private void SnapToCorrectPosition()
    {
        isPlaced = true;
        rect.anchoredPosition = correctPosition;

        // 锁死交互
        canvasGroup.blocksRaycasts = false;

        panel.OnPiecePlaced(this);
    }
}