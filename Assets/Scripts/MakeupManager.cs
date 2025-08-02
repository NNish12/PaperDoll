using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MakeupManager : MonoBehaviour
{
    public GameObject brush;
    Vector2 startPos;
    public GameObject eyebrush;
    Vector2 startEyePos;
    //заглушка под помаду
    public GameObject lipstic;
    public static MakeupManager Instance;
    public Sprite[] Lips;
    public Sprite[] BrushSprites;
    public Sprite[] EyeshadowSprites;
    private Vector2 colorPos;

    //selected принимается исходя из того когда отпустили палец
    public Sprite selectedSprite;
    public ItemType selectedType;
    //метод в котором при попадании в палитру мы смотрим на какой цвет попали курсором,
    // в таком случае принимаем в себя этот image и смотрим какой у него индекс у массива, такой же индекс передаем в наш спрайт рендерер
    public void SetColor(ButtonColor color)
    {
        int index = color.index;
        selectedType = color.itemType;
        colorPos = color.ancoredGlobalPos;

        switch (selectedType)
        {
            case ItemType.Eyeshadow:
                selectedSprite = EyeshadowSprites[index];
                StartCoroutine(AnimateAndEnableInteraction(brush, ItemType.Eyeshadow));
                break;

            case ItemType.Brush:
                selectedSprite = BrushSprites[index];
                StartCoroutine(AnimateAndEnableInteraction(brush, ItemType.Brush));
                break;

            case ItemType.Lipstick:
                selectedSprite = Lips[index];
                StartCoroutine(AnimateAndEnableInteraction(lipstic, ItemType.Lipstick));
                break;
        }
    }
    //отдельный метод для помады
    public void ResetSelectedSprite() => selectedSprite = null;
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private IEnumerator AnimateAndEnableInteraction(GameObject tool, ItemType type)
    {
        var interactable = tool.GetComponent<InteractableObject>();
        interactable.isInteractive = false;
        switch (type)
        {
            case ItemType.Brush:
                ItemActionAnimator.Instance.PlayBrushToFace(tool, colorPos, startPos);
                break;
            case ItemType.Eyeshadow:
                ItemActionAnimator.Instance.PlayBrushToFace(tool, colorPos, startEyePos);
                break;

        }
        yield return new WaitForSeconds(0.5f); //время анимации
        ItemActionAnimator.Instance.HandleDropAction(type, tool);

        yield return new WaitForSeconds(1.0f); //gодождать пока закончится анимация

        //оключаем интерактив
        interactable.isInteractive = true;
    }
    private void Start()
    {
        //взять коррдинаты родителя
        startPos = brush.GetComponent<RectTransform>().anchoredPosition;
        startEyePos = brush.GetComponent<RectTransform>().anchoredPosition;

    }
}

