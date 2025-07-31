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

        offset = item.transform.position - GetMouseWorldPos();
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
        RectTransform rectTransform = currentItem.GetComponent<RectTransform>();

        bool dragging = true;

        while (dragging)
        {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            rectTransform.position = mousePos;
        }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                Vector3 worldPos = Input.mousePosition;
                if (IsOverFaceZone(Camera.main.ScreenToWorldPoint(worldPos)))
                {
                    //если мы в зоне лица то запускаем анимацию нанесения
                    ItemActionAnimator.Instance.HandleDropAction(currentType, currentItem);
                }
                else
                {
                    // иначе оставляем на месте
                    rectTransform.position = worldPos;
                    // currentItem.transform.localPosition = Vector3.zero;
                    TabController.Instance.EnableBook(true);
                }
            }
            yield return null;
        }
    }

//проверяем, в зоне ли мы лица с помощью коллайдера и слоя маски на нем
    private bool IsOverFaceZone(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapPoint(pos, faceMask);
        return hit != null;
    }
}
