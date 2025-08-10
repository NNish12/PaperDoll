using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool canInteractWithPalette = false;
    public bool CanInteractWithPalette
    {
        get { return canInteractWithPalette; }
        set
        {
            canInteractWithPalette = value;
        }
    }
    public bool creamApplied = false;
    public void SetCreamApplied()
    {
        creamApplied = true;
        CheckBookAccess(true);
    }
    public void SetCreamReturn()
    {
        creamApplied = false;
        CheckBookAccess(false);
    }

    public void UseSponge()
    {
        ResetMakeup();
        CheckBookAccess(false);
    }

    void ResetMakeup()
    {
        GirlManager.Instance.RemoveMakeup();
    }

    public void CheckBookAccess(bool isOn)
    {
        if (creamApplied)
            UIcontroller.Instance.EnableBook(isOn);
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
