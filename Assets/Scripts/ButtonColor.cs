using UnityEngine;

public class ButtonColor : MonoBehaviour
{
    public int index;
    public ItemType itemType;
    public void ReturnIndex()
    {
        if (GameManager.Instance.canInteractWithPalette == false) return;
        // GameManager.Instance.canInteractWithPalette = false;
        Debug.Log("Return " + index);
        MakeupManager.Instance.SetColor(this);
    }
}
