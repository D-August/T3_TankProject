using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Target : MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 1000f)]
    public float hitPoints = 100f;
    [Tooltip("If object already contains a death method, uncheck this")]
    public bool utd = true;

    [Header("Explosion prefab")]
    public GameObject EXPL;
    public AudioClip a_EXPL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitPoints <= 0 && utd)
        {
            InstantiateExplosion();
            Destroy(this.gameObject);
        }
    }

    public void InstantiateExplosion()
    {
        GameObject temp = Instantiate(EXPL, transform.position, transform.rotation);
        temp.transform.SetParent(null);

        //ADD AUDIO TO HIT
        Scr_AudioCon.ac.PlaySound(a_EXPL, 1, false, temp, 1);

    }
}
