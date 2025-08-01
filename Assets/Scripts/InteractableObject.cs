
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType itemType;

    private Canvas canvas; // для правильного позиционирования
    private RectTransform rectTransform;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;

    private Vector2 startPos;
    // private Image raycast;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        eventSystem = EventSystem.current;
        // raycast.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = rectTransform.anchoredPosition;
        PointerEventData pointerEvent = new PointerEventData(eventSystem) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEvent, results);

        foreach (RaycastResult result in results)
        {
            //можно пропустить слой 3 (книга)
            if (result.gameObject.layer == 9) return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // чтобы RaycastTarget могли видеть объекты под предметом
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
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
            Debug.Log("Предмет отпущен над лицом");
            ItemActionAnimator.Instance.HandleDropAction(itemType, this.gameObject);
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