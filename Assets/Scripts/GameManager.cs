using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canInteractWithPalette = false;
    public bool creamApplied = false;
    public bool spongeUsed = false;
    public bool isFaceClean = false;

    //нужно ли явно задать все стартовые значения
    public void SetCreamApplied()
    {
        creamApplied = true;
        CheckBookAccess(false);
    }
        public void SetCreamReturn()
    {
        creamApplied = false;
        CheckBookAccess(true);
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

    public void CheckBookAccess(bool isActive)
    {
        if (creamApplied && spongeUsed)
            TabController.Instance.EnableBook(isActive);
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
