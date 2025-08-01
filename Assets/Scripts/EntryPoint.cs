using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public static EntryPoint Instance;
    public GameManager gameManager;
    public GirlManager girlManager;
    public MakeupManager makeupManager;
    public PlayerHand playerHand;
    public ItemActionAnimator itemActionAnimator;
    public FaceZone faceZone;
    public TabController tabController;
    private void Awake()
    {
        SetInstance();

        gameManager.Init();
        girlManager.Init();
        girlManager.Init();
        makeupManager.Init();
        playerHand.Init();
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
