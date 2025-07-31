using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public ItemType type;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void OnMouseDown()
    {
        if (GameManager.Instance.creamApplied) return;

        if (type == ItemType.Sponge)
        {
            GameManager.Instance.UseSponge();
            return;
        }

        ItemDragController.Instance.StartDrag(this.gameObject, type);
    }
}

public enum ItemType { Cream, Eyeshadow, Lipstick, Brush, Sponge }