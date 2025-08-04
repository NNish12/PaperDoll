using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool canInteractWithPalette = false;
    public bool CanInteractWithPalette
    {
        get { return canInteractWithPalette; }
        set
        {
            Debug.Log("can interact" + value);
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
        //здесь булевая на использование спонжа
        if (creamApplied)
            UIcontroller.Instance.EnableBook(isOn);
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
    }
}
