using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    public GameManager gameManager;
    public GirlManager girlManager;
    public MakeupManager makeupManager;
    public ItemActionAnimator itemActionAnimator;
    public FaceZone faceZone;
    public TabController tabController;
    public SpongeController spongeController;
    private void Awake()
    {
        SetInstance();
        gameManager.Init();
        spongeController.Init();
        girlManager.Init();
        makeupManager.Init();
        faceZone.Init();
        tabController.Init();
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
