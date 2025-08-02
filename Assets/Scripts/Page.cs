using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private ButtonColor[] buttonColor;
    public ItemType itemType;
    private void Start()
    {
        MatchColorsWithButtons();
    }
    public void ShowPage(bool isSelected)
    {
        gameObject.SetActive(isSelected);
    }
    private void MatchColorsWithButtons()
    {
        for (int i = 0; i < buttonColor.Length; i++)
        {
            buttonColor[i].index = i;
            buttonColor[i].itemType = this.itemType;
        }
    }
}
