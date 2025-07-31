using UnityEngine;


public class GirlManager : MonoBehaviour
{
    public static GirlManager Instance;

    [Header("Face state")]
    public GameObject Acne;

    private Sprite blush;
    private Sprite lips;
    private Sprite eyeshadow;
    [SerializeField] private SpriteRenderer srBlush;
    [SerializeField] private SpriteRenderer srLips;
    [SerializeField] private SpriteRenderer srEyeshadow;


    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void DefaultState()
    {
        ClearShadow();
        ClearLipstick();
        ClearBlush();
        ReturnAcne();
    }
    public void SetSprite()
    {

    }

    //Cream
    public void RemoveAcne()
    {
        Acne.SetActive(false);
        GameManager.Instance.isFaceClean = true;
    }

    //Eyeshadow
    public void ApplyShadow(Sprite shadow)
    {
        if (shadow != null)
            srEyeshadow.sprite = shadow;
    }

    public void ClearShadow()
    {
        if (eyeshadow != null)
            eyeshadow = null;
    }

    //Lipstic
    public void ApplyLipstick()
    {
        if (lips != null)
            srLips.sprite = lips;
    }

    public void ClearLipstick()
    {
        if (lips != null)
            lips = null;
    }

    //Blush
    public void ApplyBlush()
    {
        if (blush != null)
            srBlush.sprite = blush;
    }

    public void ClearBlush()
    {
        if (blush != null)
            blush = null;
    }

    //Полная очистка макияжа
    public void RemoveMakeup()
    {
        ClearBlush();
        ClearLipstick();
        ClearShadow();
    }
    public void ReturnAcne()
    {
        GameManager.Instance.isFaceClean = false;
        Acne.SetActive(true);
    }


}

