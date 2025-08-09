using System.Collections;
using System.Net.NetworkInformation;
using UnityEditor.Timeline.Actions;
using UnityEngine;


public class GirlManager : MonoBehaviour
{
    public static GirlManager Instance;

    [Header("Face state")]
    public GameObject Acne;
    public GameObject Dirt;
    private SpongeController sponge;
    [SerializeField] private SpriteRenderer srBlush;
    [SerializeField] private SpriteRenderer srLips;
    [SerializeField] private SpriteRenderer srEyeshadow;
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
        Acne.SetActive(false);
        Dirt.SetActive(false);
        //анимация
        GameManager.Instance.SetCreamApplied();
    }
    public void ApplyShadow(Sprite shadow)
    {
        if (shadow != null)
            StartCoroutine(FadeIn(shadow, srEyeshadow));
        CheckCosmetic();
    }

    public void ClearShadow()
    {
        if (srEyeshadow.sprite != null)
            srEyeshadow.sprite = null;
        CheckCosmetic();
    }
    public void ApplyLipstick(Sprite lips)
    {
        if (lips != null)
            StartCoroutine(FadeIn(lips, srLips));
        CheckCosmetic();
    }
    public void ClearLipstick()
    {
        if (srLips.sprite != null)
            srLips.sprite = null;
        CheckCosmetic();
    }
    public void ApplyBlush(Sprite blush)
    {
        if (blush != null)
        {
            StartCoroutine(FadeIn(blush, srBlush));
        }

        CheckCosmetic();
    }
    public void ClearBlush()
    {
        if (srBlush.sprite != null)
        {
            srBlush.sprite = null;
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
        Acne.SetActive(true);
        Dirt.SetActive(true);
        UIcontroller.Instance.EnableBook(false);
        GameManager.Instance.creamApplied = false;
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
        Debug.Log(srBlush.sprite != null && srEyeshadow.sprite != null && srLips.sprite != null);
        bool isActiveButton = srBlush.sprite != null && srEyeshadow.sprite != null && srLips.sprite != null;
        UIcontroller.Instance.SetActiveButton(isActiveButton);
    }
    private IEnumerator FadeIn(Sprite sprite, SpriteRenderer sr, float duration = 1.5f)
    {
        sr.sprite = sprite;
        Color color = sr.color;
        color.a = 0f;
        sr.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            sr.color = color;
            yield return null;
        }
        color.a = 1f;
        sr.color = color;
    }
}