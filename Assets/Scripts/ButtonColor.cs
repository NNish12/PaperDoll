using Unity.Collections;
using UnityEngine;

public class ButtonColor : MonoBehaviour
{
    public int index;
    public ItemType itemType;
    public void ReturnIndex()
    {
        // Debug.Log("GameManager.Instance.CanInteractWithPalette " + GameManager.Instance.CanInteractWithPalette);
        if (GameManager.Instance.CanInteractWithPalette == false) return;
        MakeupManager.Instance.SetColor(this);
        UIcontroller.Instance.EnableBook(false);
    }
}
