using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    public GameManager gameManager;
    public GirlManager girlManager;
    public MakeupManager makeupManager;
    public ItemAnimator itemActionAnimator;
    public FaceZone faceZone;
    public UIcontroller uiController;
    public SpongeController spongeController;
    private void Awake()
    {
        SetInstance();
        gameManager.Init();
        girlManager.Init();
        spongeController.Init();
        makeupManager.Init();
        faceZone.Init();
        uiController.Init();
        itemActionAnimator.Init();
    }
    private void SetInstance()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
