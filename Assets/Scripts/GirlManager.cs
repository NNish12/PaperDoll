using System.Net.NetworkInformation;
using UnityEngine;


public class GirlManager : MonoBehaviour
{
    public static GirlManager Instance;

    [Header("Face state")]
    public GameObject Acne;

    private Sprite blush;
    private Sprite lips;
    private Sprite eyeshadow;
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
                sponge.SetSpongeInteractable(true);
            else sponge.SetSpongeInteractable(false);
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
        Debug.Log("Удалили акне");
        Acne.SetActive(false);
        //анимация
        GameManager.Instance.SetCreamApplied();
    }
    public void ApplyShadow(Sprite shadow)
    {
        if (srEyeshadow.sprite != null)
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
        if (srLips.sprite != null)
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
        if (srBlush.sprite != null)
            srBlush.sprite = blush;
        ContainsCosmetics = true;
        CheckCosmetic();
    }
    public void ClearBlush()
    {
        if (srBlush.sprite != null)
        {
            Debug.Log("bush не равен налл иначе поменять на рендерер");
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
        if (srBlush.sprite == null && srEyeshadow.sprite == null && srLips.sprite == null)
        {
            ContainsCosmetics = false;
            // sponge.SetSpongeInteractable(false);
        }
        else
        {
            ContainsCosmetics = true;
            // sponge.SetSpongeInteractable(true);
        }

    }


}