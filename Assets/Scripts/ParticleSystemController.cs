using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public static ParticleSystemController Instance;
    private ParticleSystem ps;
    private ParticleSystemRenderer psRend;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ps = GetComponent<ParticleSystem>();
        psRend = GetComponent<ParticleSystemRenderer>();

        SetConfigure();
    }
    public void SetPlayBubbles()
    {
        StartCoroutine(PlayBubbles());
    }
    private IEnumerator PlayBubbles()
    {
        ps.Play();
        yield return new WaitForSeconds(1f);
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    private void SetConfigure()
    {
        ParticleSystem.MainModule main = ps.main;
        main.startDelay = 0f;
        main.startSpeed = 3.5f;
        main.gravityModifier = 0.03f;
        main.maxParticles = 15;
        main.loop = false;
        main.playOnAwake = false;

        ParticleSystem.EmissionModule emissionModule = ps.emission;
        emissionModule.rateOverTime = 20f;

        ParticleSystem.ShapeModule shapeModule = ps.shape;
        shapeModule.angle = 5.2f;
        shapeModule.radius = 0.75f;

        psRend.minParticleSize = 0.01f;
        psRend.maxParticleSize = 0.07f;
    }
}
