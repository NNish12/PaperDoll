using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpongeController : MonoBehaviour
{
    public static SpongeController Instance;
    private Animator animator;
    private RectTransform rectTransform;
    private Button button;
    private Vector2 startPos;
    private bool usingSponge = false;
    void Start()
    {

    }
    public void SetSpongeInteractable(bool isOn) => button.interactable = isOn;
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        SetSpongeInteractable(false);
    }
    public void UseSponge()
    {
        Debug.Log("not use, contains cosmetics " + GirlManager.Instance.ContainsCosmetics + "using sponge" + usingSponge);
        if (GirlManager.Instance.ContainsCosmetics == true && usingSponge == false)
        {
            Debug.Log("use");
            UIcontroller.Instance.EnableBook(false);
            SetSpongeInteractable(false);
            usingSponge = true;
            StartCoroutine(ApplySponge());
            //запуск анимации и корутины в которой будет установка булевой для завершения
            GirlManager.Instance.RemoveMakeup();
            UIcontroller.Instance.EnableBook(true);
        }
    }
    IEnumerator ApplySponge()
    {
        Debug.Log("animator");
        animator.Play("SpongeAnimation");
        //particlesystem
        //вставить время анимации
        yield return new WaitForSeconds(3f);
        usingSponge = false;
    }
    public void Init() { }
}
