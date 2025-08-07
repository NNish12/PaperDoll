using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemAnimator : MonoBehaviour
{
    public static ItemAnimator Instance;
    private GameManager gameManager;
    private GirlManager girlManager;
    public RectTransform handZone;
    public RectTransform creamZone;
    public RectTransform faceZone;
    public RectTransform lipZone;
    public RectTransform eyebshadowZone;
    public RectTransform brushZone;
    private RectTransform tool;
    private Vector2 startPos;
    private ItemType currentType;
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

    public void HandleDropAction(ItemType type, GameObject item)
    {
        tool = item.GetComponent<RectTransform>();
        currentType = type;
        switch (type)
        {
            case ItemType.Cream:

                if (!gameManager.creamApplied)
                    StartCoroutine(ApplyCream(item));
                break;

            case ItemType.Eyeshadow:
                if (tool == null) return;
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyShadow(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Brush:
                if (tool == null) return;
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyBlush(MakeupManager.Instance.selectedSprite)));
                break;
                
            case ItemType.Lipstick:
                if (tool == null) return;
                StartCoroutine(ApplyLipstiсAndUnlock(() => girlManager.ApplyLipstick(MakeupManager.Instance.selectedSprite)));
                break;

        }
    }
    private IEnumerator PlayLipsticSequence(RectTransform lipstic)
    {
        InteractableObject interactable = lipstic.gameObject.GetComponent<InteractableObject>();
        if (interactable != null) interactable.isInteractive = false;
        lipstic.GetComponent<Button>().interactable = false;
        startPos = lipstic.anchoredPosition;
        yield return StartCoroutine(IncreaseScale(lipstic, scaleTo: 1.2f));
        yield return StartCoroutine(MoveTo(lipstic, handZone, null, 0.5f));
        lipstic.GetComponent<ButtonColor>().SetInteractableObjectButton(true);
    }

    private IEnumerator PlayToolSequence(RectTransform tool)
    {
        InteractableObject interactable = tool.gameObject.GetComponent<InteractableObject>();
        if (interactable != null) interactable.isInteractive = false;
        startPos = tool.anchoredPosition;
        yield return StartCoroutine(IncreaseScale(tool, scaleTo: 1.2f));
        yield return StartCoroutine(MoveTo(tool, MakeupManager.Instance.button, null, 0.5f));
        yield return StartCoroutine(MoveRightLeftUI(tool, 140f, 30f, count: 1));
        yield return StartCoroutine(MoveTo(tool, handZone, null, 0.5f));
        interactable.isInteractive = true;
    }
    public void PlayToolToFace(RectTransform tool)
    {
        StartCoroutine(PlayToolSequence(tool));
    }
    public void PlayLipstic(RectTransform lipstic)
    {
        StartCoroutine(PlayLipsticSequence(lipstic));
    }

    private IEnumerator ApplyLipstiсAndUnlock(Action action)
    {
        yield return StartCoroutine(MoveTo(tool.GetComponent<RectTransform>(), lipZone, null, 0.7f));
        yield return StartCoroutine(MoveRightLeftUI(tool, 150f, 15f, count: 2));
        action();
        yield return StartCoroutine(MoveTo(tool, null, startPos, duration: 1f));
        yield return StartCoroutine(IncreaseScale(tool, scaleTo: 1f));
        tool.GetComponent<ButtonColor>().SetInteractableObjectButton(false);
        UIcontroller.Instance.EnableBook(true);
    }

    private IEnumerator ApplyAndUnlock(Action action)
    {
        tool.GetComponent<InteractableObject>().isInteractive = false;

        yield return StartCoroutine(MoveTo(tool.GetComponent<RectTransform>(), currentType == ItemType.Brush ? brushZone : eyebshadowZone, null, 0.5f));
        yield return StartCoroutine(MoveRightLeftUI(tool, 680f, 80f, count: 2));
        action();
        yield return StartCoroutine(MoveTo(tool.GetComponent<RectTransform>(), null, startPos, 1f));
        yield return StartCoroutine(IncreaseScale(tool, scaleTo: 1f));

        UIcontroller.Instance.EnableBook(true);
    }
    public void MoveToHandCreamZone(RectTransform tool)
    {
        StartCoroutine(MoveTo(tool, creamZone, null, 0.5f));
    }

    public IEnumerator MoveTo(RectTransform item, RectTransform target = null, Vector2? targetPosition = null, float duration = 1f)
    {
        Vector2 finalTargetPos;

        if (target != null)
        {
            Vector3 worldTargetPos = target.position;
            RectTransform parent = item.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                RectTransformUtility.WorldToScreenPoint(null, worldTargetPos),
                null,
                out finalTargetPos
            );

            float itemHeight = item.rect.height;
            finalTargetPos.y -= itemHeight / 2f;
        }
        else if (targetPosition.HasValue)
        {
            finalTargetPos = targetPosition.Value;
        }
        else
        {
            yield break;
        }

        Vector2 start = item.anchoredPosition;
        float t = 0f;

        while (t < duration)
        {
            item.anchoredPosition = Vector2.Lerp(start, finalTargetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        item.anchoredPosition = finalTargetPos;
    }

    private IEnumerator IncreaseScale(RectTransform target, float scaleTo = 1.2f, float duration = 0.7f)
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

    private IEnumerator ApplyCream(GameObject item)
    {
        girlManager.RemoveAcne();
        item.GetComponent<InteractableObject>().ReturnToStartPos();
        item.GetComponent<InteractableObject>().isInteractive = false;
        item.GetComponent<Image>().raycastTarget = false;
        UIcontroller.Instance.EnableBook(true);
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator MoveRightLeftUI(RectTransform tool, float speed, float distance, int count = 1)
    {
        Vector2 centerPos = tool.anchoredPosition;
        Vector2 rightPos = centerPos + Vector2.right * distance;
        Vector2 leftPos = centerPos + Vector2.left * distance;
        for (int i = 0; i < count; i++)
        {
            yield return StartCoroutine(MoveToPosition(tool, rightPos, speed));
            yield return StartCoroutine(MoveToPosition(tool, centerPos, speed));
            yield return StartCoroutine(MoveToPosition(tool, leftPos, speed));
            yield return StartCoroutine(MoveToPosition(tool, centerPos, speed));
        }
    }

    private IEnumerator MoveToPosition(RectTransform tool, Vector2 targetPos, float speed)
    {
        while (Vector2.Distance(tool.anchoredPosition, targetPos) > 0.01f)
        {
            tool.anchoredPosition = Vector2.MoveTowards(tool.anchoredPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        tool.anchoredPosition = targetPos;
    }

    public void ApplySponge()
    {
        StartCoroutine(SpongeApply());
    }
    private IEnumerator SpongeApply()
    {
        RectTransform spongeRect = SpongeController.Instance.spongeRect;
        SpongeController.Instance.usingSponge = true;
        yield return StartCoroutine(MoveTo(spongeRect, faceZone, null, 1f));
        yield return StartCoroutine(RotateInCircle(spongeRect, 690f, 40f));
        yield return StartCoroutine(MoveTo(spongeRect, null, SpongeController.Instance.startPos, 1f));
        //particlesystem
        yield return new WaitForSeconds(1f);
        SpongeController.Instance.usingSponge = false;
    }
    public IEnumerator RotateInCircle(RectTransform rect, float speed, float radius)
    {
        Vector2 center = rect.anchoredPosition;
        float angle = 0f;
        int rotations = 2;
        float totalAngle = 360f * rotations;

        while (angle < totalAngle)
        {
            angle += speed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            float x = center.x + Mathf.Cos(rad) * radius;
            float y = center.y + Mathf.Sin(rad) * radius;

            rect.anchoredPosition = new Vector2(x, y);
            yield return null;
        }
        rect.anchoredPosition = center + new Vector2(radius, 0);
    }
}
public enum HandleZone { CreamZone, Handle, Face, None }
