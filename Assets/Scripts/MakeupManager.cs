using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MakeupManager : MonoBehaviour
{
    public static MakeupManager Instance;

    [Header("Tools")]
    public GameObject brush;
    public GameObject lipstick;
    public GameObject eyebrush;

    [Header("Sprites")]
    public Sprite[] Lips;
    public Sprite[] BrushSprites;
    public Sprite[] EyeshadowSprites;

    public Vector2 startBrushPos;
    private Vector2 startEyePos;

    public Sprite selectedSprite;
    public ItemType selectedType;

    private Vector2 buttonPos;

    private void Start()
    {
        startBrushPos = brush.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        startEyePos = eyebrush.GetComponent<RectTransform>().anchoredPosition;
        SetAnimatorEnabled(false);
    }

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SetColor(ButtonColor button)
    {
        if (!GameManager.Instance.CanInteractWithPalette) return;

        selectedType = button.itemType;
        buttonPos = button.GetComponent<RectTransform>().anchoredPosition;

        switch (selectedType)
        {
            case ItemType.Eyeshadow:
                selectedSprite = EyeshadowSprites[button.index];
                StartCoroutine(UseTool(eyebrush, ItemType.Eyeshadow, startEyePos, button.GetComponent<RectTransform>()));
                break;

            case ItemType.Brush:
                selectedSprite = BrushSprites[button.index];
                StartCoroutine(UseTool(brush, ItemType.Brush, startBrushPos, button.GetComponent<RectTransform>()));
                break;

            case ItemType.Lipstick:
                selectedSprite = Lips[button.index];
                Vector2 startLipPos = lipstick.GetComponent<RectTransform>().anchoredPosition;
                StartCoroutine(UseTool(lipstick, ItemType.Lipstick, startLipPos, button.GetComponent<RectTransform>()));
                break;
        }

        GameManager.Instance.CanInteractWithPalette = false;
    }

    private IEnumerator UseTool(GameObject toolChild, ItemType type, Vector2 startPos, RectTransform target)
    {
        var interactable = toolChild.GetComponent<InteractableObject>();
        if (interactable != null) interactable.isInteractive = false;

        string animationName = type == ItemType.Lipstick ? "ApplyLipstick" : "ApplyPalette";
        GameObject toolParent = toolChild.transform.parent.gameObject;
        RectTransform toolParentRect = toolParent.GetComponent<RectTransform>();

        Vector2 localTargetPos = GetTargetPosition(target, toolParentRect);
        localTargetPos = new Vector2(localTargetPos.x, localTargetPos.y - 150f);
        ItemAnimator.Instance.PlayToolToFace(toolParent, toolChild, localTargetPos, startPos, animationName);

        yield return new WaitForSeconds(10f);
    }


    public void ResetSelectedSprite()
    {
        if (selectedSprite != null)
            selectedSprite = null;
    }

    private void SetAnimatorEnabled(bool isOn)
    {
        eyebrush.GetComponent<Animator>().enabled = isOn;
    }
    private Vector2 GetTargetPosition(RectTransform fromRect, RectTransform toolParentRect)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, fromRect.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(toolParentRect.parent as RectTransform, screenPoint, null, out Vector2 localPoint);
        return localPoint;
    }

}
