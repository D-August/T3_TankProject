using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Key : MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 1000f)]
    public float hitPoints = 1f;

    [Header("Porta")]
    public GameObject linkedObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            Destroy(linkedObject);
            Destroy(this.gameObject);
        }
    }
}
