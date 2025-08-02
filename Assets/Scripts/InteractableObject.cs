
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
    public bool isInteractive = false;


    private Vector2 startPos;
    //массив всех интерактивных добавить в entrypoint
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
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
            //анимация переноса на нейтраль
            //дальше можно тянуть
            isInteractive = true;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractive) return;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractive) return;

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