using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TankTurr : MonoBehaviour
{
    [Header("Self Variables")]
    public float speed = 3.0f;
    public float damage = 10f;

    [Header("Target Variables")]
    public GameObject target = null;
    [Tooltip("Player's Last Position Known")]
    public Vector3 lastKnownPos = Vector3.zero;
    public Quaternion lookAtRot;
    public Quaternion canAtRot;

    [Header("Reset Position")]
    private Quaternion orRot;
    private Quaternion orCanRot;
    private float tReset = 0;
    [Range(0, 60f)]
    public float tRR = 3f;

    [Header("Cannons")]
    public List<GameObject> cannons = new List<GameObject>();
    public GameObject cans;

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

    [Header("State Machine")]
    public PossibleStates curState = PossibleStates.ON_REST;
    private PossibleStates prvState;

    // Awake, Start & Update
    void Start()
    {
        orRot = transform.rotation;
        orCanRot = cannons[0].transform.rotation;

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
        

        if (target)
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
                // OLD
                /*if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        lookAtRot = Quaternion.LookRotation(new Vector3(lastKnownPos.x, transform.position.y, lastKnownPos.z) - transform.position);
                        canAtRot = Quaternion.LookRotation(new Vector3(transform.position.x, lastKnownPos.y, lastKnownPos.z) - transform.position);
                    }
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, speed * Time.deltaTime);
                cans.transform.rotation = Quaternion.RotateTowards(cans.transform.rotation, canAtRot, (speed * 0.1f) * Time.deltaTime);*/

                // NEW
                if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        Vector3 targetDirection = lastKnownPos - transform.position;
                        float singleStep = speed * Time.deltaTime;
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(targetDirection.x, 0, targetDirection.z), singleStep, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                        cans.transform.rotation = Quaternion.LookRotation(newDirection);
                    }              
                }

                Shooting();
                break;

            case PossibleStates.ON_REST:
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, (speed * 1.5f) * Time.deltaTime);
                //cans.transform.rotation = Quaternion.RotateTowards(cans.transform.rotation, orCanRot, (speed * 1.5f) * Time.deltaTime);
                tReset = 0;
                break;

            case PossibleStates.STUNNED:
                // OLD
                transform.rotation = Quaternion.RotateTowards(transform.rotation, orRot, speed * Time.deltaTime);
                cans.transform.rotation = Quaternion.RotateTowards(cans.transform.rotation, orCanRot,  Time.deltaTime);

                if (transform.rotation == orRot)
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
                // OLD
                /*if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        lookAtRot = Quaternion.LookRotation(new Vector3(lastKnownPos.x, transform.position.y, lastKnownPos.z) - transform.position);
                        canAtRot = Quaternion.LookRotation(new Vector3(transform.position.x, lastKnownPos.y, lastKnownPos.z) - transform.position);
                    }
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRot, speed * Time.deltaTime);
                cans.transform.rotation = Quaternion.RotateTowards(cans.transform.rotation, canAtRot, (speed * 0.1f) * Time.deltaTime);*/

                // NEW
                if (target)
                {
                    if (lastKnownPos != target.transform.position)
                    {
                        lastKnownPos = target.transform.position;
                        Vector3 targetDirection = lastKnownPos - transform.position;
                        float singleStep = speed * Time.deltaTime;
                        Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(targetDirection.x, 0, targetDirection.z), singleStep, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);
                        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                        cans.transform.rotation = Quaternion.LookRotation(newDirection);
                    }
                }
                break;
        }

        foreach (GameObject i in cannons)
        {
            RaycastHit hit;
            int layerMask = 1 << 12;
            layerMask = ~layerMask;

            if (Physics.Raycast(i.transform.position, cannons[0].transform.forward, out hit, range, layerMask) && curState != PossibleStates.STUNNED)
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
            int layerMask = 1 << 12;
            layerMask = ~layerMask;

            bool isHit = Physics.Raycast(i.transform.position, cannons[0].transform.forward, out hit, range, layerMask);

            if (curState == PossibleStates.TARGETING && cooldown >= coolTime && isHit && target_locked)
            {
                if (hit.transform.tag == "Player")
                {
                    GameObject temp;

                    try
                    {
                        hit.collider.gameObject.GetComponent<Scr_Controls_PROT>().CallDamage(damage);

                        // Comentar Depois
                        Debug.Log("HIT TORRETA, Minus: " + damage.ToString());
                        Debug.Log("Vida restante do Tank: " + hit.collider.gameObject.GetComponent<Scr_Controls_PROT>().hitPoints.ToString());
                    }
                    catch
                    {
                        hit.collider.gameObject.GetComponentInParent<Scr_Controls_PROT>().CallDamage(damage);

                        // Comentar Depois
                        Debug.Log("HIT TORRETA, Minus: " + damage.ToString());
                        Debug.Log("Vida restante do Tank: " + hit.collider.gameObject.GetComponentInParent<Scr_Controls_PROT>().hitPoints.ToString());
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
