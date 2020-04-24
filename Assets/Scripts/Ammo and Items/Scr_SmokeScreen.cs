using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SmokeScreen : MonoBehaviour
{
    

    [Header("Sistema de Partículas")]
    public ParticleSystem ps;
    [Range(0f, 15f)]
    public float maxTimer = 0;
    private float timer = 0;

    [Header("Colisor")]
    public SphereCollider sc;
    [Range(0f, 100f)]
    public float maxRange = 0;
    [Range(0f, 50f)]
    public float gSpeed = 0;


    void Start()
    {
        sc.radius = 0f;
    }

    [System.Obsolete]
    void Update()
    {
        if (sc.radius < 45f) sc.radius += gSpeed * Time.deltaTime;

        if (timer >= maxTimer) {
            ps.enableEmission = false;
            if (ps.particleCount <= 0) Destroy(this.gameObject);
        } else timer += Time.deltaTime;
        
    }
}
