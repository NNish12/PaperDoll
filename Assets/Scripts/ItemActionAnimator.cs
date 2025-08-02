using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionAnimator : MonoBehaviour
{
    public static ItemActionAnimator Instance;
    private GameManager gameManager;
    private GirlManager girlManager;
    public Animator animator;
    public RectTransform handZone;

    private GameObject currentTool;
    private Vector2 startPos;
    public void Init()
    {
        if (Instance != this && Instance != null)
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
        if (type == ItemType.Cream && !GameManager.Instance.creamApplied)
        {
            StartCoroutine(ApplyCream(item));
            return;
        }
        switch (type)
        {
            case ItemType.Eyeshadow:
                StartCoroutine(ApplyEyeshadow());
                break;
            case ItemType.Brush:
                // StartCoroutine(ApplyBlush());
                break;
            case ItemType.Lipstick:
                StartCoroutine(ApplyLipstick());
                break;
        }
    }

    IEnumerator ApplyCream(GameObject item)
    {
        girlManager.RemoveAcne();
        //анимация сброса на лицо
        item.GetComponent<InteractableObject>().ReturnToStartPos();
        item.GetComponent<Image>().raycastTarget = false;
        // анимация возврата на место
        item.GetComponent<InteractableObject>().isInteractive = false;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator ApplyEyeshadow()
    {
        yield return new WaitForSeconds(0.7f);
        // girlManager.ApplyShadow();
        gameManager.canInteractWithPalette = true;
    }
    IEnumerator ApplyLipstick()
    {
        yield return new WaitForSeconds(0.7f);
        // girlManager.ApplyLipstick();
        gameManager.canInteractWithPalette = true;
    }
    IEnumerator ApplyBlush(Sprite sprite)
    {
        yield return new WaitForSeconds(0.7f);
        girlManager.ApplyBlush(sprite);
        gameManager.canInteractWithPalette = true;
    }
    public void PlayBrushToFace(GameObject tool, Vector2 targetPos, Vector2 startPos)
    {
        //плавное перемещение к зоне 
        StartCoroutine(PlayBrush(tool, targetPos, startPos));
        tool.GetComponent<InteractableObject>().isInteractive = true;
    }
    private IEnumerator PlayBrush(GameObject tool, Vector2 targetPos, Vector2 startPos)
    {
        animator = tool.GetComponent<Animator>();

        currentTool = tool;
        this.startPos = startPos;
        StartCoroutine(IncreeseScale(tool.GetComponent<RectTransform>()));
        yield return new WaitForSeconds(1f);
        //возвратом секунд

        //проблема с перемещением
        yield return StartCoroutine(MoveToPosition(tool.transform.parent.GetComponent<RectTransform>(), targetPos, 0.5f));
        animator.Play("ApplyPalette");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(MoveToPosition(tool.transform.parent.GetComponent<RectTransform>(), handZone.anchoredPosition, 0.5f));
        // тут передача управления потом уже применение и возврат

    }
    private IEnumerator SetColorToFace()
    {
        animator.Play("ApplyToFace");
        yield return new WaitForSeconds(1f); //cколько сек
        yield return StartCoroutine(MoveToPosition(currentTool.transform.parent.GetComponent<RectTransform>(), startPos, 0.5f));

        //включить интерактивность
        currentTool.GetComponent<InteractableObject>().isInteractive = true;
        //после применения обязательно 
        GameManager.Instance.canInteractWithPalette = true;
    }

    IEnumerator WaitForReturnToCase()
    {
        animator.Play("IncreeseScale");
        yield return new WaitForSeconds(1f);

    }

    IEnumerator MoveToPosition(RectTransform rectTransform, Vector2 targetPos, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;
    }

    public IEnumerator IncreeseScale(RectTransform target, float scaleTo = 1.2f, float duration = 0.3f)
    {
        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * scaleTo;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale;
        // elapsed = 0f;
        // while (elapsed < duration)
        // {
        //     target.localScale = Vector3.Lerp(targetScale, startScale, elapsed / duration);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }
        // target.localScale = startScale;
    }


}
