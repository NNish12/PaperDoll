using UnityEngine;

public class Page : MonoBehaviour
{
    public void ShowPage(bool isSelected)
    {
        //selected
        gameObject.SetActive(isSelected);
    }
}
