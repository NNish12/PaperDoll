using System.Net.NetworkInformation;
using UnityEngine;


public class GirlManager : MonoBehaviour
{
    public static GirlManager Instance;

    [Header("Face state")]
    public GameObject Acne;
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
        ReturnAcne();
    }
    public void RemoveAcne()
    {
        Acne.SetActive(false);
        //анимация
        GameManager.Instance.SetCreamApplied();
    }
    public void ApplyShadow(Sprite shadow)
    {
        if (shadow != null)
            srEyeshadow.sprite = shadow;
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
            srLips.sprite = lips;
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
            srBlush.sprite = blush;
        // ContainsCosmetics = true;
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
        sponge.SetSpongeInteractable(false);
    }
    public void ReturnAcne()
    {
        Acne.SetActive(true);
    }
    //тут дублирование
    public void CheckCosmetic()
    {
        //тут объект может быть null
        if (srBlush.sprite != null || srEyeshadow.sprite != null || srLips.sprite != null)
        {
            ContainsCosmetics = true;
            // sponge.SetSpongeInteractable(false);
        }
        else
        {
            ContainsCosmetics = false;
            // sponge.SetSpongeInteractable(true);
        }

    }


}