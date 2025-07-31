using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapHandler : MonoBehaviour
{
    public static TapHandler Instance;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private PointerEventData pointerEvent;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("нажали");
            pointerEvent = new PointerEventData(eventSystem) { position = Input.mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEvent, results);

            foreach (RaycastResult result in results)
            {
                InteractableObject interactable = result.gameObject.GetComponent<InteractableObject>();
                Debug.Log("мы в цикле полученного рейкаста");
                if (interactable != null)
                {
                    Debug.Log("нажали и попали в интерактив");
                    interactable.OnMouseDown();
                }
            }
        }
    }
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
