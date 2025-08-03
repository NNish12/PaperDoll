using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionAnimator : MonoBehaviour
{
    public static ItemActionAnimator Instance;

    private GameManager gameManager;
    private GirlManager girlManager;

    public RectTransform handZone;
    public RectTransform creamZone;
    private GameObject currentTool;
    private Vector2 startToolPos;

    public float y = 160f;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        gameManager = GameManager.Instance;
        girlManager = GirlManager.Instance;
    }

    public void PlayToolToFace(GameObject toolParent, GameObject toolChild, Vector2 targetPos, Vector2 startPos, string animationName)
    {
        currentTool = toolChild;
        startToolPos = startPos;

        RectTransform parentRect = toolParent.GetComponent<RectTransform>();
        RectTransform toolRect = toolChild.GetComponent<RectTransform>();

        StartCoroutine(PlayToolSequence(toolChild, parentRect, toolRect, targetPos, animationName));
    }

    private IEnumerator PlayToolSequence(GameObject toolChild, RectTransform parentRect, RectTransform toolRect, Vector2 targetPos, string animationName)
    {
        Animator animator = toolChild.GetComponent<Animator>();
        animator.enabled = false;

        // yield return StartCoroutine(IncreaseScale(toolRect));
        yield return StartCoroutine(MoveTo(parentRect, targetPos, 0.5f));

        yield return new WaitForSeconds(0.2f);

        animator.enabled = true;
        animator.Play(animationName);
        //заглушка для конца анимации, добавить секунды
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(MoveTo(parentRect, handZone.anchoredPosition, 0.5f));
        yield return new WaitForSeconds(1f);
        toolRect.gameObject.GetComponent<InteractableObject>().isInteractive = true;
        animator.enabled = false;
    }
    public void MoveToHandZone(RectTransform tool)
    {
        Vector2 target = creamZone.anchoredPosition;
        StartCoroutine(MoveTo(tool, target, 0.5f));
    }

    private IEnumerator MoveTo(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        Vector2 start = rectTransform.anchoredPosition;
        float t = 0f;

        while (t < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(start, targetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }

    private IEnumerator IncreaseScale(RectTransform target, float scaleTo = 1.2f, float duration = 1f)
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * scaleTo;
        float t = 0f;

        while (t < duration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale;
    }

    public void HandleDropAction(ItemType type, GameObject item)
    {
        switch (type)
        {
            case ItemType.Cream:
                if (!gameManager.creamApplied)
                    StartCoroutine(ApplyCream(item));
                break;

            case ItemType.Eyeshadow:
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyShadow(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Lipstick:
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyLipstick(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Brush:
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyBlush(MakeupManager.Instance.selectedSprite)));
                break;
        }
    }

    private IEnumerator ApplyCream(GameObject item)
    {
        girlManager.RemoveAcne();
        item.GetComponent<InteractableObject>().ReturnToStartPos();
        item.GetComponent<InteractableObject>().isInteractive = false;
        item.GetComponent<Image>().raycastTarget = false;
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ApplyAndUnlock(System.Action action)
    {
        yield return new WaitForSeconds(0.7f);
        action?.Invoke();
        gameManager.canInteractWithPalette = true;
    }
}
