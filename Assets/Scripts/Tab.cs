using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private bool isSelected = false;
    public Image defaultImage;
    public Image selectedImage;

    public void Init()
    {
        SetSelected(isSelected);
    }

    public void SetSelected(bool isSelected)
    {
        defaultImage.gameObject.SetActive(!isSelected);
        selectedImage.gameObject.SetActive(isSelected);
    }
}
