using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrackingSystem : MonoBehaviour
{
    [Header("Self Variables")]
    [Range(0, 15f)]
    public float speed = 3.0f;
    public float damage = 10f;
    
    [Header("Target Variables")]
    public GameObject target = null;
    [Tooltip("Player's Last Position Known")]
    public Vector3 lastKnownPos = Vector3.zero;
    public Quaternion lookAtRot;

    [Header("Reset Position")]
    private Quaternion orRot;
    private float tReset = 0;
    [Range(0, 60f)]
    public float tRR = 3f;

    [Header("Cannons")]
    public List<GameObject> cannons = new List<GameObject>();

    [Header("Shot Variables")]
    public float range = 100f;
    private float cooldown = 0;
    [Range(0, 60f)]
    public float coolTime = 1.5f;


    [Header("Explosion")]
    public GameObject Explosion;
    public enum PossibleStates
    {
        STUNNED,
        ON_REST,
        TARGETING,
        TARGET_LOCK
    }
    [Header("State Machine")]
    public PossibleStates curState;
     
    // Awake, Start & Update
    void Start()
    {
        orRot = transform.rotation;
        
    }

    void Update()
    {
        if (target)
        {
            tReset = 0;

            if (lastKnownPos != target.transform.position)
            {
                lastKnownPos = target.transform.position;
                lookAtRot = Quaternion.LookRotation(lastKnownPos - transform.position);
            }

            if (transform.rotation != lookAtRot)
            {
                curState = PossibleStates.TARGETING;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, speed * Time.deltaTime);
            }
        }
        else
        {
            if (tReset >= tRR)
            {
                curState = PossibleStates.ON_REST;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, (speed * 1.5f) * Time.deltaTime);
                tReset = 0;
            }
            else tReset += Time.deltaTime;
        }


        foreach(GameObject i in cannons)
        {
            RaycastHit hit;

            if (Physics.Raycast(i.transform.position, this.transform.forward, out hit, range))
            {
                // Comentar Depois
                Debug.DrawLine(i.transform.position, hit.point, Color.green);

                if(hit.transform.tag == "Player") curState = PossibleStates.TARGET_LOCK; else curState = PossibleStates.TARGETING;

            }
        }

        Shooting();

    }

    public bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }

        this.target = target;

        return true;
    }

    public void Shooting()
    {
        foreach (GameObject i in cannons)
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(i.transform.position, this.transform.forward, out hit, range);

            if (curState == PossibleStates.TARGET_LOCK && cooldown >= coolTime && isHit)
            {
                if (hit.transform.tag == "Player")
                {
                    hit.collider.gameObject.GetComponent<Scr_Controls_PROT>().CallDamage(damage);
                    GameObject temp = Instantiate(Explosion, hit.point, transform.rotation);
                    temp.transform.SetParent(null);

                    // Comentar Depois
                    Debug.Log("HIT TORRETA, Minus: " + damage.ToString());
                    Debug.Log("Vida restante do Tank: " + hit.collider.gameObject.GetComponent<Scr_Controls_PROT>().hitPoints.ToString());

                    cooldown = 0;
                }
            }
        }

        if (cooldown <= coolTime) cooldown += Time.deltaTime;
    }
}
