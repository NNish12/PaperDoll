using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIcontroller : MonoBehaviour
{
    public static UIcontroller Instance;
    [SerializeField] private Image nonInteractableField;
    [SerializeField] private List<Tab> tabs;
    [SerializeField] private List<Page> pages;
    [SerializeField] private List<Button> uiButtons;
    [SerializeField] private Button enableButton;
    [SerializeField] private Button disableButton;

    private int currentIndex = -1;
    public Vector2 MousePos { get; set; }

    private void Start()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            int index = i; //иначе замыкание
            uiButtons[i].onClick.AddListener(() => OnTabClicked(index));
        }

        OnTabClicked(0); //активируем первую вкладку по умолчанию
    }

    private void OnTabClicked(int index)
    {
        if (index == currentIndex) return;

        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].SetSelected(i == index);
            pages[i].ShowPage(i == index);
        }

        currentIndex = index;
        MakeupManager.Instance.ResetSelectedSprite();
    }

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        EnableBook(false);
    }

    public void EnableBook(bool isOn)
    {
        GameManager.Instance.CanInteractWithPalette = isOn;
        nonInteractableField.enabled = !isOn;
    }

    public void Exit()
    {
        Application.Quit();
    }
}