using Unity.VisualScripting;
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
    public UIRaycastDebugger raycaster;
    private void Awake()
    {
        SetInstance();
        gameManager.Init();
        uiController.Init();
        makeupManager.Init();
        girlManager.Init();
        spongeController.Init();

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
