using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType itemType;
    public bool isInteractive = false;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;
    private Vector2 startPos;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        eventSystem = EventSystem.current;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = rectTransform.anchoredPosition;
        PointerEventData pointerEvent = new PointerEventData(eventSystem) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEvent, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == 9) return;
        }
        if (itemType == ItemType.Cream && !GameManager.Instance.creamApplied)
        {
            isInteractive = true;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //захват с сильной задержкой и дерганьем
        if (!isInteractive) return;
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractive) return;

        RectTransform parentRect = rectTransform.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (FaceZone.IsOverZone(eventData.position))
        {

            isInteractive = false;
            Debug.Log("Предмет отпущен над лицом");
            ItemAnimator.Instance.HandleDropAction(itemType, this.gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPos;
        }
    }
    public void ReturnToStartPos()
    {
        rectTransform.anchoredPosition = startPos;
    }
}

public enum ItemType { Cream, Eyeshadow, Lipstick, Brush, Sponge }
