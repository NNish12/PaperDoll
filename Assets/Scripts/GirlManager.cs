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
    public bool containsCosmetics = false;

    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        sponge = SpongeController.Instance;
    }
    public void DefaultState()
    {
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
        if (shadow != null)
            srEyeshadow.sprite = shadow;
    }

    public void ClearShadow()
    {
        if (eyeshadow != null)
            eyeshadow = null;
        CheckCosmetic();
    }
    public void ApplyLipstick(Sprite lips)
    {
        if (lips != null)
            srLips.sprite = lips;
    }
    public void ClearLipstick()
    {
        if (lips != null)
            lips = null;
        CheckCosmetic();
    }
    public void ApplyBlush(Sprite blush)
    {
        if (blush != null)
            srBlush.sprite = blush;
        containsCosmetics = true;
    }
    public void ClearBlush()
    {
        if (blush != null)
            blush = null;
        CheckCosmetic();
    }
    public void RemoveMakeup()
    {
        ClearBlush();
        ClearLipstick();
        ClearShadow();
        containsCosmetics = false;
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
            containsCosmetics = false;
            sponge.SetSpongeInteractable(false);
        }
        else
        {
            containsCosmetics = true;
            sponge.SetSpongeInteractable(true);
        }

    }


}