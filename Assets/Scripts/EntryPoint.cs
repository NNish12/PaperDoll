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
    public UIRaycastDebugger raycaster;
    private void Awake()
    {
        SetInstance();
        gameManager.Init();
        uiController.Init();
        makeupManager.Init();
        girlManager.Init();
        faceZone.Init();
        itemActionAnimator.Init();
        raycaster.Init();
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
