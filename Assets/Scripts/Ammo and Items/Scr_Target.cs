using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Target : MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 1000f)]
    public float hitPoints = 100f;

    [Header("Explosion prefab")]
    public GameObject EXPL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoints <= 0)
        {
            GameObject temp = Instantiate(EXPL, transform.position, transform.rotation);
            temp.transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }
}
