using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EMP : MonoBehaviour
{
    [Range(0f, 15f)]
    public float maxTimer = 0;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (timer >= maxTimer)
        {
            Destroy(this.gameObject);
        }
        else timer += Time.deltaTime;

    }
}
