using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MakeupManager : MonoBehaviour
{
    public static MakeupManager Instance;
    public Transform chestPosition;
    public Image[] LipsticsPalette;
    public Sprite[] Lips;
    public Image[] BrushePalette;
    public Sprite[] BrushSprites;
    public Image[] EyeshadowPalette;
    public Sprite[] EyeshadowSprites;


    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartShadowRoutine(Vector3 colorPosition)
    {
        StartCoroutine(ShadowRoutine(colorPosition));
    }

    public void StartLipstickRoutine(Vector3 itemPosition)
    {
        StartCoroutine(LipstickRoutine(itemPosition));
    }

    IEnumerator ShadowRoutine(Vector3 colorPosition)
    {
        GameManager.Instance.canInteractWithPalette = false;

        yield return PlayerHand.Instance.PlayPickBrushAnimation(colorPosition);
        PlayerHand.Instance.transform.position = chestPosition.position;
        //делегат изцчить почему в {} 
        yield return PlayerHand.Instance.WaitForDragToFace(() =>
        {
            StartCoroutine(ApplyShadow());
        });
    }

    IEnumerator ApplyShadow()
    {
        yield return PlayerHand.Instance.PlayApplyShadowAnimation();
        // GirlManager.Instance.ApplyShadow();
        PlayerHand.Instance.ReturnToDefault();
        GameManager.Instance.canInteractWithPalette = true;
    }

    IEnumerator LipstickRoutine(Vector3 itemPosition)
    {
        GameManager.Instance.canInteractWithPalette = false;

        yield return PlayerHand.Instance.PlayTakeLipstick(itemPosition);
        PlayerHand.Instance.transform.position = chestPosition.position;

        yield return PlayerHand.Instance.WaitForDragToFace(() =>
        {
            StartCoroutine(ApplyLipstick());
        });
    }

    IEnumerator ApplyLipstick()
    {
        yield return PlayerHand.Instance.PlayApplyLipstickAnimation();
        // GirlManager.Instance.ApplyLipstick();
        PlayerHand.Instance.ReturnToDefault();
        GameManager.Instance.canInteractWithPalette = true;
    }
}

