using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpongeController : MonoBehaviour
{
    public static SpongeController Instance;
    public RectTransform spongeRect;
    public Vector2 startPos;
    private Button button;
    public bool usingSponge = false;
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

        spongeRect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        SetSpongeInteractable(false);
        startPos = spongeRect.anchoredPosition;
    }
    public void UseSponge()
    {
        if (GirlManager.Instance.ContainsCosmetics == true && usingSponge == false)
        {
            UIcontroller.Instance.EnableBook(false);
            SetSpongeInteractable(false);
            ItemAnimator.Instance.ApplySponge();
            GirlManager.Instance.RemoveMakeup();
            UIcontroller.Instance.EnableBook(true);
        }
    }

    public void Init() { }
}
