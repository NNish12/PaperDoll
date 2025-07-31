using UnityEngine;

public class FaceZone : MonoBehaviour
{
    public static FaceZone Instance;
    public Collider2D faceCollider;
    public void Init()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        
        faceCollider = GetComponent<Collider2D>();
        //нужно ли 
    }
}
