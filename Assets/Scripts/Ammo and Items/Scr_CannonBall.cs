using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CannonBall: MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 50)]
    public float speed = 16f;
    [Range(0, 50)]
    public float damage = 20f;

    [Header("Prefabs")]
    [Tooltip("Shield's Prefab")]
    public GameObject sPrfb;
    [Tooltip("Smoke's Prefab")]
    public GameObject smPrfb;
    [Tooltip("EMP Prefab")]
    public GameObject ePref;

    [Header("Distance Variables")]
    public bool calcPos = true;
    [Tooltip("Default is 1.15, the standard size of a cube")]
    [Range(0, 100f)]
    public float meters = 1.15f;
    private Vector3 iniPos;

    [Header("Explosions Prefabs")]
    public List<GameObject> ECUSUPUROZIONS = new List<GameObject>();

    [Header("Audio Files")]
    public List<AudioClip> ac_list = new List<AudioClip>();

    public enum bulletType
    {
        COMMOM,
        SHIELD,
        SMOKE,
        EMP
    }

    public bulletType bt;
    
    void Start()
    {
        iniPos = transform.position;
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (transform.position.y <= GameObject.Find("EOTW").transform.position.y) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (bt)
        {
            case bulletType.COMMOM:
                if (other.gameObject.name == "Shield" || other.transform.CompareTag("Terrain") || other.transform.CompareTag("Des_OBJ"))
                {
                    switch (other.gameObject.transform.tag)
                    {
                        case "Des_OBJ":
                            try
                            {
                                try
                                {
                                    other.GetComponent<Scr_Target>().hitPoints -= damage;
                                    Debug.Log("Vida Restante Alvo: " + other.GetComponent<Scr_Target>().hitPoints.ToString());
                                } catch
                                {
                                    other.GetComponentInParent<Scr_Target>().hitPoints -= damage;
                                    Debug.Log("Vida Restante Alvo: " + other.GetComponentInParent<Scr_Target>().hitPoints.ToString());
                                }
                            }
                            catch
                            {
                                other.GetComponent<Scr_Key>().hitPoints -= damage;
                            }

                            // Depois Comentar Debug
                            
                            break;
                    }

                    CalculateDistance();
                    CollideExplosion("COMMON");
                    Destroy(this.gameObject);
                }
                break;
            case bulletType.SHIELD:
                if (other.gameObject.name == "Terrain" || other.transform.CompareTag("Des_OBJ"))
                {
                    Vector3 tempVec = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    GameObject temp = GameObject.Instantiate(sPrfb, tempVec, new Quaternion(sPrfb.transform.rotation.x, sPrfb.transform.rotation.y, sPrfb.transform.rotation.z, sPrfb.transform.rotation.w));
                    temp.transform.eulerAngles += new Vector3(0, this.transform.eulerAngles.y, 0);

                    //ADD AUDIO TO HIT
                    //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);

                    CalculateDistance();
                    Destroy(this.gameObject);
                }
                break;
            case bulletType.SMOKE:
                if (other.gameObject.name == "Terrain" || other.gameObject.name == "Enemy" || other.transform.CompareTag("Des_OBJ"))
                {
                    Vector3 tempVec = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    GameObject temp = GameObject.Instantiate(smPrfb, tempVec, new Quaternion(0,0,0,1));
                    temp.transform.localScale *= 4;

                    //ADD AUDIO TO HIT
                    //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);

                    CalculateDistance();
                    Destroy(this.gameObject);
                }
                break;
            case bulletType.EMP:
                if (other.gameObject.name == "Terrain" || other.gameObject.name == "Enemy" || other.transform.CompareTag("Des_OBJ"))
                {
                    Vector3 tempVec = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    GameObject temp = GameObject.Instantiate(ePref, tempVec, new Quaternion(0, 0, 0, 1));
                    
                    //ADD AUDIO TO HIT
                    //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);
                    
                    CalculateDistance();
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    public void CollideExplosion(string type)
    {
        switch (type)
        {
            default:
                GameObject temp = Instantiate(ECUSUPUROZIONS[0], transform.position, transform.rotation);
                
                //ADD AUDIO TO EXPLOSION
                Scr_AudioCon.ac.PlaySound(ac_list[0], .05f, false, temp);
                
                temp.transform.SetParent(null);
                break;
        }
    }

    public void CalculateDistance()
    {
        float temp = Vector3.Distance(iniPos, transform.position);
        temp /= meters;

        // Depois Comentar Debug
        if (calcPos) Debug.Log("Distancia Percorrida = " + temp.ToString());
    }
}
