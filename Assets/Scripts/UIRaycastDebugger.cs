using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRaycastDebugger : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public static UIRaycastDebugger Instance;
    public EventSystem eventSystem;
    public bool raycast = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && raycast)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count == 0)
            {
                Debug.Log("Ничего не под курсором");
            }
            else
            {
                Debug.Log("Объекты под курсором:");
                foreach (var result in results)
                {
                    Debug.Log(result.gameObject.name);
                }
            }
        }
    }

    public void Init()
    {
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
    }
}

