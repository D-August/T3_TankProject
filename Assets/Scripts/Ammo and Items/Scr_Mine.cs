using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Mine : MonoBehaviour
{
    [Range(0f, 150f)]
    public float damage = 0;

    [Header("Explosions")]
    public GameObject ECUSUPUROZION;

    [Header("PS")]
    public ParticleSystem ps;

    void Start()
    {
        
    }
    void Update()
    {
        ps = GetComponentInChildren<ParticleSystem>();

        if (ps)
        {
            if(GameObject.Find("Tank (Player)")) ps.trigger.SetCollider(0, GameObject.Find("Tank (Player)").transform.Find("MineDetector").GetComponent<SphereCollider>());
        }
        if(GetComponent<Scr_Target>().hitPoints <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Scr_Controls_PROT>().CallDamage(damage);
            
            // Depois Comentar Debugs
            Debug.Log("EXPLOSION!!!! Minus: " + damage.ToString());
            Debug.Log("Vida Restante Tank: " + other.GetComponent<Scr_Controls_PROT>().hitPoints.ToString());

            Death();
        }
        
    }

    public void Death()
    {
        //ADD AUDIO
        //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);

        GameObject temp = Instantiate(ECUSUPUROZION, transform.position, transform.rotation);
        temp.transform.SetParent(null);
        Destroy(this.gameObject);
    }
}
