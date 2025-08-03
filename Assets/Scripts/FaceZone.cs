using UnityEngine;

public class FaceZone : MonoBehaviour
{
    public static FaceZone Instance;
    private RectTransform rectTransform;
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    public static bool IsOverZone(Vector2 screenPosition)
    {
        if (Instance == null) return false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Instance.rectTransform,
            screenPosition,
            null,
            out Vector2 localPoint);

        return Instance.rectTransform.rect.Contains(localPoint);
    }
}
