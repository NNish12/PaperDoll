using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionAnimator : MonoBehaviour
{
    public static ItemActionAnimator Instance;
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void HandleDropAction(ItemType type, GameObject item)
    {
        //отдельно для крема, потому что у него нет передающего спрайта
        if (type == ItemType.Cream && !GameManager.Instance.creamApplied)
        {
            StartCoroutine(ApplyCream(item));
            return;
        }
        //тут надо сделать метод приема конкретного спрайта и передачи с конкретной палетки 
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        //если сбрасываем мышь с предметом типа
        switch (type)
        {
            case ItemType.Eyeshadow:
                StartCoroutine(ApplyEyeshadow(sprite));
                break;
            case ItemType.Lipstick:
                StartCoroutine(ApplyLipstick(sprite));
                break;
        }
    }

    IEnumerator ApplyCream(GameObject item)
    {
        GirlManager.Instance.RemoveAcne();
        //анимация сброса
        item.GetComponent<InteractableObject>().ReturnToStartPos();
        item.GetOrAddComponent<Image>().raycastTarget = false;

        yield return new WaitForSeconds(1f);

    }

    //сделаны только тени
    IEnumerator ApplyEyeshadow(Sprite sprite)
    {
        yield return new WaitForSeconds(0.7f);
        GirlManager.Instance.ApplyShadow(sprite);
        GameManager.Instance.canInteractWithPalette = true;
    }

    IEnumerator ApplyLipstick(Sprite sprite)
    {
        yield return new WaitForSeconds(0.7f);
        GirlManager.Instance.ApplyLipstick(sprite);
        GameManager.Instance.canInteractWithPalette = true;
    }
    IEnumerator ApplyBlush(Sprite sprite)
    {
        yield return new WaitForSeconds(0.7f);
        GirlManager.Instance.ApplyBlush(sprite);
        GameManager.Instance.canInteractWithPalette = true;
    }
}
