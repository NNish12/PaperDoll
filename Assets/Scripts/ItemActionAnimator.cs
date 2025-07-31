using System.Collections;
using UnityEngine;

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
        //если сбрасываем мышь с предметом типа
        switch (type)
        {
            case ItemType.Cream:
                StartCoroutine(ApplyCream());
                break;
            case ItemType.Eyeshadow:
                StartCoroutine(ApplyEyeshadow(item.GetComponent<SpriteRenderer>().sprite));
                break;
            case ItemType.Lipstick:
                StartCoroutine(ApplyLipstick());
                break;
        }
    }

    IEnumerator ApplyCream()
    {
        yield return new WaitForSeconds(1f);
        //анимация сброса
        GirlManager.Instance.RemoveAcne();
        GameManager.Instance.SetCreamApplied();

    }

//сделаны только тени
    IEnumerator ApplyEyeshadow(Sprite sprite)
    {
        yield return new WaitForSeconds(0.7f);
        GirlManager.Instance.ApplyShadow(sprite);
        GameManager.Instance.canInteractWithPalette = true;
    }

    IEnumerator ApplyLipstick()
    {
        yield return new WaitForSeconds(0.7f);
        GirlManager.Instance.ApplyLipstick();
        GameManager.Instance.canInteractWithPalette = true;
    }
}
