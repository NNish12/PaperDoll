using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIcontroller : MonoBehaviour
{
    public static UIcontroller Instance;
    [SerializeField] private Image nonInteractableField; 
    [SerializeField] private List<Tab> tabs;
    [SerializeField] private List<Page> pages;
    [SerializeField] private List<Button> uiButtons; //те, на которые кликают

    // это должен быть uicontroller
    private int currentIndex = -1;

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
        //если переключение то сбросим спрайт 
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
        GameManager.Instance.canInteractWithPalette = isOn;
        nonInteractableField.enabled = !isOn;
    }
}