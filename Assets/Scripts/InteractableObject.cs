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
    private Vector3 dragOffset;

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
        if (!isInteractive) return;
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;

        RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, eventData.position, null, out Vector3 globalMousePos))
        {
            dragOffset = rectTransform.position - globalMousePos;
        }
        else
        {
            dragOffset = Vector3.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractive) return;

        RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, eventData.position, null, out Vector3 globalMousePos))
        {
            rectTransform.position = globalMousePos + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (FaceZone.IsOverZone(eventData.position))
        {
            isInteractive = false;
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
