using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scr_TowerMinion : MonoBehaviour
{
    public enum Main_StateMachine
    {
        STANDBY,
        LOOKING,
        MOVING_TE /* Moving Towards Enemy */
    }

    [Header("Stats")]
    [Tooltip("Movement Speed")]
    public float MoveSpd = 0;
    [Tooltip("Damage Dealt to Player on Colision")]
    public float xplDmg = 0;
    [Tooltip("Explosion Prefab")]
    public GameObject pref_expl;

    [Header("StateMachineBehaviour")]
    public Main_StateMachine sm;

    [Header("Audios")]
    public List<AudioClip> ac_list = new List<AudioClip>();

    public GameObject tower;
    public GameObject targetPlayer;

    // COMEÇA AQUI
    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;


    // Use this for initialization
    void OnEnable()
    {
        tower = GetComponentInParent<Scr_TowerSpawn>().gameObject;
        wanderRadius = tower.GetComponentInChildren<SphereCollider>().radius;

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        
    }

    // Update is called once per frame
    void Update()
    {
        Death();

        switch (sm)
        {
            case Main_StateMachine.STANDBY:
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(tower.transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
                break;
            case Main_StateMachine.MOVING_TE:
                MovetoPP();
                break;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void MovetoPP()
    {
        if (agent.isOnNavMesh && targetPlayer)
        {
            Vector3 destiny = targetPlayer.transform.position;
            agent.isStopped = false;
            agent.enabled = true;
            agent.speed = MoveSpd;

            agent.destination = destiny;
        } 
    }

    public void Death()
    {
        if(GetComponent<Scr_Target>().hitPoints <= 0)
        {
            try
            {
                GetComponentInParent<Scr_TowerSpawn>().enemies.Remove(gameObject);
            } catch { }
            
            GameObject temp = Instantiate(pref_expl, transform);
            temp.transform.SetParent(null);

            //ADD AUDIO
            //Scr_AudioCon.ac.PlaySound(ac_list[*ADD*], 1, false, temp);

            Destroy(gameObject);
        }
    }

    public void Suicide(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            try { other.gameObject.GetComponent<Scr_Controls_PROT>().CallDamage(xplDmg); }
            catch { other.gameObject.GetComponentInParent<Scr_Controls_PROT>().CallDamage(xplDmg); }
            GetComponent<Scr_Target>().hitPoints = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Suicide(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            targetPlayer = other.GetComponentInParent<Scr_Controls_PROT>().gameObject;
            sm = Main_StateMachine.MOVING_TE;
        }

        if (other.transform.CompareTag("EMP"))
        {
            GetComponent<Scr_Target>().hitPoints = 0;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            targetPlayer = null;
            sm = Main_StateMachine.STANDBY;
        }
    }
}
