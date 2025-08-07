using Unity.Collections;
using UnityEngine;

public class ButtonColor : MonoBehaviour
{
    public int index;
    public ItemType itemType;
    private InteractableObject interactableObject;
    public void ReturnIndex()
    {
        if (GameManager.Instance.CanInteractWithPalette == false) return;
        MakeupManager.Instance.SetColor(this);
        UIcontroller.Instance.EnableBook(false);
    }

    void Awake()
    {
        if (itemType == ItemType.Lipstick)
        {
            interactableObject = GetComponent<InteractableObject>();
            interactableObject.enabled = false;
        }
    }

    public void SetInteractableObjectButton(bool ison)
    {
        interactableObject.enabled = ison;
        interactableObject.isInteractive = ison;
    }
}
