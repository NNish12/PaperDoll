using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    public int index;
    public ItemType itemType;
    public Vector2 ancoredGlobalPos;
    private RectTransform rectTransform;
    public Canvas canvas;


    public void ReturnIndex()
    {
        if (GameManager.Instance.canInteractWithPalette == false) return;
        GameManager.Instance.canInteractWithPalette = false;
        // ancoredPos = GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("Return " + index);
        MakeupManager.Instance.SetColor(this);
        Debug.Log("colorPos: " + ancoredGlobalPos);
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        ancoredGlobalPos = GetGlobalAnchoredPosition(rectTransform, canvas);
    }

    public static Vector2 GetGlobalAnchoredPosition(RectTransform target, Canvas rootCanvas)
    {
        Vector2 globalPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(null, target.position),
            rootCanvas.worldCamera,
            out globalPoint);

        return globalPoint;
    }
}
