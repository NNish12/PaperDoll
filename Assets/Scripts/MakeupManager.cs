using System.Collections;

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
    public ItemType selectedType;
    public RectTransform currentLipstic;
    public Vector2 startDragItemPos;
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
                // startDragItemPos = startEyePos;
                selectedSprite = EyeshadowSprites[colorButton.index];
                //тут был старт драг айтем позицио
                StartCoroutine(UseTool(eyebrush, button));
                break;

            case ItemType.Brush:
                // startDragItemPos = startBrushPos;
                selectedSprite = BrushSprites[colorButton.index];
                StartCoroutine(UseTool(brush, button));
                break;

            case ItemType.Lipstick:
                currentLipstic = button;
                selectedSprite = Lips[colorButton.index];
                StartCoroutine(UseLipstic(currentLipstic, currentLipstic.GetComponent<RectTransform>(), ItemAnimator.Instance.handZone));
                break;
        }

        GameManager.Instance.CanInteractWithPalette = false;
    }
    private IEnumerator UseLipstic(RectTransform tool, RectTransform start, RectTransform target)
    {
        //кнопка перемещается в handzone
        //становится интерактивной
        //перемещаетс
        //применяется
        //возвращается
        Vector2 startPos = start.anchoredPosition;
        Vector2 localTargetPos = GetTargetPosition(target, tool);
        localTargetPos = new Vector2(localTargetPos.x, localTargetPos.y - 150f);
        ItemAnimator.Instance.PlayToolToFace(tool);

        yield return new WaitForSeconds(10f);
    }


    private IEnumerator UseTool(RectTransform tool, RectTransform target)
    {
        InteractableObject interactable = tool.gameObject.GetComponent<InteractableObject>();
        if (interactable != null) interactable.isInteractive = false;
        //здесь как раз надо будет добавить смещение на 150f
        // Vector2 targetPosOffset = new Vector2(targetPos.x, targetPos.y - 150f);
        ItemAnimator.Instance.PlayToolToFace(tool);

        yield return new WaitForSeconds(10f);
    }


    public void ResetSelectedSprite()
    {
        if (selectedSprite != null)
            selectedSprite = null;
    }

    private Vector2 GetTargetPosition(RectTransform fromRect, RectTransform toolParentRect)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, fromRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(toolParentRect.parent as RectTransform, screenPoint, null, out Vector2 localPoint);
        return localPoint;
    }

}
