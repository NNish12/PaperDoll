using System.Collections;
using UnityEngine;

public class ItemDragController : MonoBehaviour
{
    public static ItemDragController Instance;

    private Camera mainCamera;
    private GameObject currentItem;
    private ItemType currentType;
    private Vector3 offset;

    public Transform faceZone;
    public LayerMask faceMask;
    private RectTransform rectTransform;
    private Vector2 startPos;

    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        mainCamera = Camera.main;
    }

    public void StartDrag(GameObject item, ItemType type)
    {
        currentItem = item;
        currentType = type;

        Debug.Log(this.startPos);
        rectTransform = item.GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        // offset = item.transform.position - GetMouseWorldPos();
        StartCoroutine(DragRoutine());
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 10f;
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private IEnumerator DragRoutine()
    {
        //если книга неинтерактивна
        TabController.Instance.EnableBook(false);
        rectTransform = currentItem.GetComponent<RectTransform>();
        Debug.Log("DragRoutine");
        bool dragging = true;

        while (dragging)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 localPoint;
                //кнвертируем экранную позицию мыши в локальную позицию относительно родительского RectTransform
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, Input.mousePosition, null, out localPoint);

                rectTransform.anchoredPosition = localPoint;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;

                Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (IsOverFaceZone(worldPos))
                {
                    Debug.Log("Крем в зоне лица");
                    ItemActionAnimator.Instance.HandleDropAction(currentType, currentItem);
                    rectTransform.anchoredPosition = startPos;
                }
                else //иначе если и убрать метод ниже
                {
                    rectTransform.anchoredPosition = startPos;
                    TabController.Instance.EnableBook(false);
                }
            }
            yield return null;
        }
    }
    //проверяем, в зоне ли мы лица с помощью коллайдера и слоя на нем
    private bool IsOverFaceZone(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos, faceMask);
        Debug.Log("IsOverFaceZone" + hit != null);
        return hit != null;
    }
}
