using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool canInteractWithPalette = false;
    public bool creamApplied = false;


    //нужно ли явно задать все стартовые значения
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
