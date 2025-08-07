using System.Collections;

using UnityEngine;
using UnityEngine.UI;


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
    public ItemType selectedType;
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
        Debug.Log(selectedType);
        button = colorButton.GetComponent<RectTransform>();

        switch (selectedType)
        {
            case ItemType.Eyeshadow:
                selectedSprite = EyeshadowSprites[colorButton.index];
                StartCoroutine(UseTool(eyebrush, button));
                break;

            case ItemType.Brush:
                selectedSprite = BrushSprites[colorButton.index];
                StartCoroutine(UseTool(brush, button));
                break;

            case ItemType.Lipstick:
                currentLipstic = button.GetComponent<RectTransform>();
                selectedSprite = Lips[colorButton.index];
                StartCoroutine(UseLipstic(currentLipstic));
                break;
        }

        GameManager.Instance.CanInteractWithPalette = false;
    }
    private IEnumerator UseLipstic(RectTransform button)
    {
        ItemAnimator.Instance.PlayLipstic(button);
        //yield никак не проявляется, нужно ли убрать?
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator UseTool(RectTransform tool, RectTransform target)
    {
        //здесь как раз надо будет добавить смещение на 150f
        // Vector2 targetPosOffset = new Vector2(targetPos.x, targetPos.y - 150f);
        ItemAnimator.Instance.PlayToolToFace(tool);
        //yield никак не проявляется, нуно ли убрать?
        yield return new WaitForSeconds(10f);
    }

    public void ResetSelectedSprite()
    {
        if (selectedSprite != null)
            selectedSprite = null;
    }
}
