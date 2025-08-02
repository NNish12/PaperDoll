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
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        SetSpongeInteractable(false);
    }
    public void SetSpongeInteractable(bool isOn) => button.interactable = isOn;
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void UseSponge()
    {
        if (GirlManager.Instance.containsCosmetics == true && usingSponge == false)
        {
            GameManager.Instance.canInteractWithPalette = false;
            SetSpongeInteractable(false);
            usingSponge = true;
            StartCoroutine(ApplySponge());
            //запуск анимации и корутины в которой будет установка булевой для завершения
            //корутина
            GirlManager.Instance.RemoveMakeup();

            //не нужно
            rectTransform.anchoredPosition = startPos;
            GameManager.Instance.canInteractWithPalette = true;
        }
    }
    IEnumerator ApplySponge()
    {
        animator.Play("SpongeAnimation");
        //анимация
        //particlesystem
        //вставить время анимации
        yield return new WaitForSeconds(3f);
        usingSponge = false;
        SetSpongeInteractable(true);
    }
}
