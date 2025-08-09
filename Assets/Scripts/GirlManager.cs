using System.Collections;
using UnityEngine;


public class GirlManager : MonoBehaviour
{
    public static GirlManager Instance;

    [Header("Face states")]
    private SpongeController sponge;
    [SerializeField] private SpriteRenderer srBlush;
    [SerializeField] private SpriteRenderer srLips;
    [SerializeField] private SpriteRenderer srEyeshadow;
    [SerializeField] private SpriteRenderer srAcne;
    [SerializeField] private SpriteRenderer srDirt;
    private bool isFirstStart = true;
    private bool containsCosmetics = false;
    public bool ContainsCosmetics
    {
        get { return containsCosmetics; }
        set
        {
            if (value == true)
            {
                sponge.SetSpongeInteractable(true);
                containsCosmetics = value;
            }
            else
            {
                sponge.SetSpongeInteractable(false);
                containsCosmetics = value;
            }
        }
    }
    private void SetNullMakeUp()
    {
        srBlush.sprite = null;
        srLips.sprite = null;
        srEyeshadow.sprite = null;
    }
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        sponge = SpongeController.Instance;
        DefaultState();
    }
    public void DefaultState()
    {
        SetNullMakeUp();
        RemoveMakeup();
        ReturnAcneAndDirt();
    }
    public void RemoveAcne()
    {
        StartCoroutine(FadeIn(srAcne, 0f, isNullSprite: false));
        StartCoroutine(FadeIn(srDirt, 0f, isNullSprite: false));
        GameManager.Instance.SetCreamApplied();
    }
    public void ApplyShadow(Sprite shadow)
    {
        if (shadow != null)
            StartCoroutine(FadeIn(srEyeshadow, 1f, shadow));
        CheckCosmetic();
    }

    public void ClearShadow()
    {
        if (srEyeshadow.sprite != null)
        {
            StartCoroutine(FadeIn(srEyeshadow, 0f, duration: 1.8f));
        }
        CheckCosmetic();
    }
    public void ApplyLipstick(Sprite lips)
    {
        if (lips != null)
            StartCoroutine(FadeIn(srLips, 1f, lips));
        CheckCosmetic();
    }
    public void ClearLipstick()
    {
        if (srLips.sprite != null)
        {
            StartCoroutine(FadeIn(srLips, 0f, duration: 1.8f));
        }
        CheckCosmetic();
    }
    public void ApplyBlush(Sprite blush)
    {
        if (blush != null)
        {
            StartCoroutine(FadeIn(srBlush, 1f, blush));
        }

        CheckCosmetic();
    }
    public void ClearBlush()
    {
        if (srBlush.sprite != null)
        {
            StartCoroutine(FadeIn(srBlush, 0f, duration: 1.8f));
        }
        CheckCosmetic();
    }
    public void RemoveMakeup()
    {
        ClearBlush();
        ClearLipstick();
        ClearShadow();
        ContainsCosmetics = false;
    }
    public void ReturnAcneAndDirt()
    {
        SetAlpha(srAcne, 1f);
        SetAlpha(srDirt, 1f);
        UIcontroller.Instance.EnableBook(false);
        GameManager.Instance.creamApplied = false;
    }
    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
    public void CheckCosmetic()
    {
        if (srBlush.sprite != null || srEyeshadow.sprite != null || srLips.sprite != null)
        {
            ContainsCosmetics = true;
        }
        else
        {
            ContainsCosmetics = false;
        }

        if (!isFirstStart)
        {
            bool isActiveButton = srBlush.sprite != null && srEyeshadow.sprite != null && srLips.sprite != null;
            UIcontroller.Instance.SetActiveButton(isActiveButton);
        }
        isFirstStart = false;
    }

    private IEnumerator FadeIn(SpriteRenderer sr, float targetAlpha, Sprite sprite = null, float duration = 1.5f, bool isNullSprite = true)
    {
        Color color = sr.color;

        if (sprite != null)
        {
            if (sr.sprite != null)
            {
                float startAlpha = color.a;
                float halfDuration = duration / 2f;
                float elapsed = 0f;

                while (elapsed < halfDuration)
                {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Lerp(startAlpha, 0f, elapsed / halfDuration);
                    sr.color = color;
                    yield return null;
                }

                sr.sprite = sprite;
                elapsed = 0f;

                while (elapsed < halfDuration)
                {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, targetAlpha, elapsed / halfDuration);
                    sr.color = color;
                    yield return null;
                }
                color.a = targetAlpha;
                sr.color = color;
            }
            else
            {
                sr.sprite = sprite;
                color.a = 0f;
                sr.color = color;

                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, targetAlpha, elapsed / duration);
                    sr.color = color;
                    yield return null;
                }
                color.a = targetAlpha;
                sr.color = color;
            }
        }
        else
        {
            if (sr.sprite != null)
            {
                float startAlpha = color.a;
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                    sr.color = color;
                    yield return null;
                }

                color.a = 0f;
                sr.color = color;
                if (isNullSprite)
                    sr.sprite = null;
            }
        }
    }

}