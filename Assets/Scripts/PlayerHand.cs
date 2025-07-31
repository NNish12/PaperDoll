using System;
using System.Collections;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    //ЭТО АНИМАТОР КОНТРОЛЛЕР

    //или перенос предмета 
    //разделить ответсвенность
    public static PlayerHand Instance;

    public Transform defaultPos, creamMidPos, faceZone;
    public Animator animator;

    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public IEnumerator PlayTakeCreamAnimation()
    {
        animator.Play("TakeCream");
        yield return new WaitForSeconds(1f);
        transform.position = creamMidPos.position;
    }

    public IEnumerator WaitForDragToFace(Action onSuccess)
    {
        bool dragging = true;
        Vector3 offset = transform.position - Input.mousePosition;

        while (dragging)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                transform.position = new Vector3(newPos.x, newPos.y, 0);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                if (IsOverFaceZone())
                {
                    onSuccess.Invoke();
                }
                else
                {
                    //вернуть обратно
                    transform.position = creamMidPos.position;
                    GameManager.Instance.canInteractWithPalette = true;
                }
            }
            yield return null;
        }
    }

    bool IsOverFaceZone()
    {
        Collider2D faceCollider = FaceZone.Instance.GetComponent<Collider2D>();
        return faceCollider.OverlapPoint(transform.position);
    }

    public IEnumerator PlayApplyCreamAnimation()
    {
        animator.Play("ApplyCream");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayReturnCreamAnimation()
    {
        animator.Play("ReturnCream");
        yield return new WaitForSeconds(1f);
        transform.position = defaultPos.position;
    }
    public IEnumerator PlayPickBrushAnimation(Vector3 brushPosition)
    {
        // Двигаем руку к кисточке и играем анимацию "взять кисточку"
        transform.position = brushPosition;
        animator.Play("PickBrush");
        yield return new WaitForSeconds(1f); // длительность анимации
    }

    public IEnumerator PlayApplyShadowAnimation()
    {
        animator.Play("ApplyShadow");
        yield return new WaitForSeconds(0.7f); // быстрая анимация
    }

    public IEnumerator PlayTakeLipstick(Vector3 lipstickPosition)
    {
        transform.position = lipstickPosition;
        animator.Play("TakeLipstick");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayApplyLipstickAnimation()
    {
        animator.Play("ApplyLipstick");
        yield return new WaitForSeconds(1f);
    }

    public void ReturnToDefault()
    {
        transform.position = defaultPos.position;
        animator.Play("Idle");
    }

}

