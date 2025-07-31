using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canInteractWithPalette = false;
    public bool creamApplied = false;
    public bool spongeUsed = false;
    // public bool isFaceClean = false;

    //нужно ли явно задать все стартовые значения
    public void SetCreamApplied()
    {
        creamApplied = true;
        // isFaceClean = true
        CheckBookAccess(true);
    }
    public void SetCreamReturn()
    {
        creamApplied = false;
        CheckBookAccess(false);
    }

    public void UseSponge()
    {
        spongeUsed = true;
        ResetMakeup();
        CheckBookAccess(false);
    }

    void ResetMakeup()
    {
        GirlManager.Instance.RemoveMakeup();
    }

    public void CheckBookAccess(bool isOn)
    {
        //здесь булевая на использование спонжа
        // if (creamApplied && spongeUsed)
        if (creamApplied)
            TabController.Instance.EnableBook(isOn);
        //потом надо поменять на активный
    }
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
