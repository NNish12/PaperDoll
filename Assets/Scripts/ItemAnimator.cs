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
    private RectTransform tool;
    private Vector2 startPos;

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
    public void PlayToolToFace(RectTransform tool)
    {
        StartCoroutine(PlayToolSequence(tool));
    }

    private IEnumerator PlayToolSequence(RectTransform tool)
    {
        startPos = tool.anchoredPosition;
        yield return StartCoroutine(IncreaseScale(tool, scaleTo: 1.2f));
        yield return StartCoroutine(MoveTo(tool, MakeupManager.Instance.button, null, 0.5f));
        yield return StartCoroutine(MoveRightLeftUI(tool, 100f, 50f));
        yield return StartCoroutine(MoveTo(tool, handZone, null, 0.5f));
        tool.gameObject.GetComponent<InteractableObject>().isInteractive = true;
    }
    public void MoveToHandCreamZone(RectTransform tool)
    {
        StartCoroutine(MoveTo(tool, creamZone, null, 0.5f));
    }
    private IEnumerator ApplyLipstic(RectTransform rectTransform, Vector2 targetPos, float duration = 1f)
    {
        //куда таргет?
        rectTransform.GetComponent<Button>().enabled = false;
        yield return StartCoroutine(MoveTo(rectTransform, handZone, null, duration));
        rectTransform.GetComponent<InteractableObject>().enabled = true;
    }
    private IEnumerator ApplyAndUnlock(Action action)
    {
        //применяем корутину с apply animation
        //применяется спрайт
        //возвращаем на место 
        tool.GetComponent<InteractableObject>().isInteractive = false;
        yield return StartCoroutine(MoveTo(tool.GetComponent<RectTransform>(), faceZone, null, 1.5f));
        yield return StartCoroutine(MoveRightLeftUI(tool, 100f, 50f));
        action();
        yield return StartCoroutine(MoveTo(tool.GetComponent<RectTransform>(), null, startPos, 1f));
        yield return StartCoroutine(IncreaseScale(tool, scaleTo: 1f));

        UIcontroller.Instance.EnableBook(true);
    }

    private IEnumerator MoveTo(RectTransform item, RectTransform target = null, Vector2? targetPosition = null, float duration = 1f)
    {
        Vector2 finalTargetPos;

        if (target != null)
        {
            Vector3 worldTargetPos = target.position;
            RectTransform parent = item.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, RectTransformUtility.WorldToScreenPoint(null, worldTargetPos), null, out finalTargetPos);
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
        tool = item.GetComponent<RectTransform>();
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

            case ItemType.Lipstick:
                if (tool == null) return;
                StartCoroutine(ApplyAndUnlock(() => girlManager.ApplyLipstick(MakeupManager.Instance.selectedSprite)));
                break;

            case ItemType.Brush:
                if (tool == null) return;
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
        UIcontroller.Instance.EnableBook(true);
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator MoveRightLeftUI(RectTransform tool, float speed, float distance)
    {
        int repeats = 2;
        Vector2 startPos = tool.anchoredPosition;

        for (int i = 0; i < repeats; i++)
        {
            Vector2 targetRight = startPos + Vector2.right * distance;
            while (Vector2.Distance(tool.anchoredPosition, targetRight) > 0.01f)
            {
                tool.anchoredPosition = Vector2.MoveTowards(tool.anchoredPosition, targetRight, speed * Time.deltaTime);
                yield return null;
            }

            while (Vector2.Distance(tool.anchoredPosition, startPos) > 0.01f)
            {
                tool.anchoredPosition = Vector2.MoveTowards(tool.anchoredPosition, startPos, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
public enum HandleZone { CreamZone, Handle, Face, None }
