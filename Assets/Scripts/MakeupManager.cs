using UnityEngine;

public class MakeupManager : MonoBehaviour
{
    public static MakeupManager Instance;

    [Header("Tools")]
    public RectTransform brush;
    public RectTransform lipstick;
    public RectTransform eyebrush;

    [Header("Sprites")]
    public Sprite[] Lips;
    public Sprite[] BrushSprites;
    public Sprite[] EyeshadowSprites;
    public Sprite selectedSprite;
    private ItemType selectedType;
    public RectTransform currentLipstic;
    public RectTransform button;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SetColor(ButtonColor colorButton)
    {
        if (!GameManager.Instance.CanInteractWithPalette) return;

        selectedType = colorButton.itemType;
        button = colorButton.GetComponent<RectTransform>();

        switch (selectedType)
        {
            case ItemType.Eyeshadow:
                selectedSprite = EyeshadowSprites[colorButton.index];
                ItemAnimator.Instance.PlayToolToFace(eyebrush);
                break;

            case ItemType.Brush:
                selectedSprite = BrushSprites[colorButton.index];
                ItemAnimator.Instance.PlayToolToFace(brush);
                break;

            case ItemType.Lipstick:
                currentLipstic = button.GetComponent<RectTransform>();
                selectedSprite = Lips[colorButton.index];
                ItemAnimator.Instance.PlayLipstic(currentLipstic);
                break;
        }
        GameManager.Instance.CanInteractWithPalette = false;
    }

    public void ResetSelectedSprite()
    {
        if (selectedSprite != null)
            selectedSprite = null;
    }
}
