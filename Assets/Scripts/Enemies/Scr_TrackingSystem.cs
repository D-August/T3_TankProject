using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TrackingSystem : MonoBehaviour
{
    [Header("Self Variables")]
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
    public bool target_locked = false;
    public float range = 25f;
    private float cooldown = 0;
    [Range(0, 60f)]
    public float coolTime = 1.5f;


    [Header("EMP Variables")]
    private float emptime = 0;
    [Range(0, 60f)]
    public float empCool = 5f;

    [Header("Explosion")]
    public GameObject Explosion;
    public enum PossibleStates
    {
        STUNNED,
        ON_REST,
        TARGETING,
        SMOKED
    }

    [Header("Audios")]
    public List<AudioClip> ac_list = new List<AudioClip>();

    [Header("State Machine")]
    public PossibleStates curState = PossibleStates.ON_REST;
    private PossibleStates prvState;
     
    // Awake, Start & Update
    void Start()
    {
        orRot = transform.rotation;
        
    }

    void Update()
    {      
        StateController();
    }

    // Trigger Collision Events
    private void OnTriggerEnter(Collider other)
    {
        switch (other.transform.tag)
        {
            case "EMP":

                break;
            case "Smoke":
                prvState = curState;
                curState = PossibleStates.SMOKED;
                break;

        }
    }
    private void OnTriggerStay(Collider other)
    {
        switch (other.transform.tag)
        {
            case "EMP":
                curState = PossibleStates.STUNNED;
                break;
            case "Smoke":
                curState = PossibleStates.SMOKED;
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.transform.tag)
        {
            case "EMP":

                break;
            case "Smoke":
                curState = prvState;
                break;
        }
    }

    // Controllers
        // State Machine Controller
    public void StateController()
    {
        if(target)
        {
            if (transform.rotation != lookAtRot && curState != PossibleStates.STUNNED)
            {
                curState = PossibleStates.TARGETING;
            }
        }
        else
        {
            if (tReset >= tRR)
            {
                curState = PossibleStates.ON_REST;
            }
            else tReset += Time.deltaTime;
            // Debug.Log(tReset.ToString());
        }

        switch (curState)
        {
            case PossibleStates.TARGETING:
                if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        lookAtRot = Quaternion.LookRotation(lastKnownPos - transform.position);
                    }
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, speed * Time.deltaTime);

                Shooting();

                break;

            case PossibleStates.ON_REST:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, (speed * 1.5f) * Time.deltaTime);
                tReset = 0;
                break;

            case PossibleStates.STUNNED:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, (speed * 1.5f) * Time.deltaTime);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, speed * Time.deltaTime);
                transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);

                if ( transform.rotation == orRot)
                {
                    if (emptime >= empCool)
                    {
                        emptime = 0;
                        curState = PossibleStates.ON_REST;
                    }
                    else emptime += Time.deltaTime;
                }
                break;

            case PossibleStates.SMOKED:
                if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        lookAtRot = Quaternion.LookRotation(lastKnownPos - transform.position);
                    }
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, speed * Time.deltaTime);
                break;
        }

        foreach (GameObject i in cannons)
        {
            RaycastHit hit;
            int layerMask = 1 << 12;
            layerMask = ~layerMask;

            if (Physics.Raycast(i.transform.position, this.transform.forward, out hit, range, layerMask) && curState != PossibleStates.STUNNED)
            {
                // Comentar Depois
                Debug.DrawLine(i.transform.position, hit.point, Color.green);

                if (hit.transform.tag == "Player" && target) target_locked = true; else target_locked = false;

            }
        }
    }

    // Actions Methods
        // Set Turret Target
    public bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }

        this.target = target;

        return true;
    }
        // Turret Start Shooting
    public void Shooting()
    {
        foreach (GameObject i in cannons)
        {
            RaycastHit hit;
            int layerMask = 1 << 2;
            layerMask = ~layerMask;

            bool isHit = Physics.Raycast(i.transform.position, this.transform.forward, out hit, range, layerMask);

            if (curState == PossibleStates.TARGETING && cooldown >= coolTime && isHit && target_locked)
            {
                //ADD AUDIO
                //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, gameObject, Random.Range(.75f, 2f));

                if (hit.transform.tag == "Player")
                {
                    GameObject temp;

                    try
                    {
                        hit.collider.gameObject.GetComponent<Scr_Controls_PROT>().CallDamage(damage);
                        //ADD AUDIO
                        //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, hit.collider.gameObject);
                    }
                    catch
                    {
                        hit.collider.gameObject.GetComponentInParent<Scr_Controls_PROT>().CallDamage(damage);
                        //ADD AUDIO
                        //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, hit.collider.gameObject);
                    }

                    temp = Instantiate(Explosion, hit.point, transform.rotation);
                    temp.transform.SetParent(null);

                    cooldown = 0;
                }
            }
        }

        if (cooldown <= coolTime) cooldown += Time.deltaTime;
    }

}
