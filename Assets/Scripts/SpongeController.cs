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
            ApplySponge();
            GirlManager.Instance.RemoveMakeup();
            UIcontroller.Instance.EnableBook(true);
        }
    }
    public void ApplySponge()
    {
        StartCoroutine(SpongeApply());
    }
    private IEnumerator SpongeApply()
    {
        RectTransform spongeRect = SpongeController.Instance.spongeRect;
        SpongeController.Instance.usingSponge = true;

        yield return StartCoroutine(MoveTo(spongeRect, ItemAnimator.Instance.faceZone, null, 3f));
        Debug.Log("1");
        yield return StartCoroutine(RotateInCircle(spongeRect, 2f, 40f));
        Debug.Log("2");
        yield return StartCoroutine(MoveTo(spongeRect, null, SpongeController.Instance.startPos, 3f));
        Debug.Log("3");
        //particlesystem
        yield return new WaitForSeconds(1f);
        SpongeController.Instance.usingSponge = false;
        Debug.Log("4");
    }
    public IEnumerator RotateInCircle(RectTransform rect, float speed, float radius)
    {
        Vector2 center = rect.anchoredPosition;
        float angle = 0f;
        int rotations = 2;
        float totalAngle = 360f * rotations;

        while (angle < totalAngle)
        {
            angle += speed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;
            float x = center.x + Mathf.Cos(rad) * radius;
            float y = center.y + Mathf.Sin(rad) * radius;

            rect.anchoredPosition = new Vector2(x, y);
            yield return null;
        }

        rect.anchoredPosition = center + new Vector2(radius, 0);
    }


    public IEnumerator MoveTo(RectTransform item, RectTransform target = null, Vector2? targetPosition = null, float duration = 1f)
    {
        Vector2 finalTargetPos;

        if (target != null)
        {
            Vector3 worldTargetPos = target.position;
            RectTransform parent = item.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, RectTransformUtility.WorldToScreenPoint(null, worldTargetPos), null, out finalTargetPos);
        }
        else if (targetPosition.HasValue)
        {
            finalTargetPos = targetPosition.Value;
        }
        else
        {
            yield break;
        }

        Vector2 start = item.anchoredPosition;
        float t = 0f;

        while (t < duration)
        {
            item.anchoredPosition = Vector2.Lerp(start, finalTargetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        item.anchoredPosition = finalTargetPos;
    }


    public void Init() { }
}
