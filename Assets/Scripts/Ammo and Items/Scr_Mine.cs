using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Mine : MonoBehaviour
{
    [Range(0f, 150f)]
    public float damage = 0;

    [Header("Explosions")]
    public GameObject ECUSUPUROZION;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Scr_Controls_PROT>().CallDamage(damage);
            
            // Depois Comentar Debugs
            Debug.Log("EXPLOSION!!!! Minus: " + damage.ToString());
            Debug.Log("Vida Restante Tank: " + other.GetComponent<Scr_Controls_PROT>().hitPoints.ToString());
            
            GameObject temp =  Instantiate(ECUSUPUROZION, transform.position, transform.rotation);
            temp.transform.SetParent(null);
            Destroy(this.gameObject);
        }
        
    }
}
