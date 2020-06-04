using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scr_EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5f;

    [Header("Components")]
    [Tooltip("Enemy's NavMesh Agent (Automatic set)")]
    public NavMeshAgent agent;
    [Tooltip("Area the enemy will be able to go")]
    public GameObject freeRoamAA;

    private Vector3 destination;
    public float wanderTime = 3;
    private float time = 0;


    public float stuntime = 5f;
    private float stime;
    public enum States
    {
        Standby,
        Chasing,
        Stunned,
        Smoked
    }

    [Header("State Machine")]
    public States sm;

    // Start is called before the first frame update
    void Start()
    {
        CreateFRAA();
    }

    // Update is called once per frame
    void Update()
    {
        StateController();
        Death();
    }
    
    public void StateController()
    {
        switch(sm)
        {
            case States.Standby:
                StateMovement();


                if (GetComponentInChildren<Scr_TankTurr>().target)
                {
                    sm = States.Chasing;
                    time = 0;
                }
                StateMovement();
                break;

            case States.Chasing:
                if (!GetComponentInChildren<Scr_TankTurr>().target) sm = States.Standby;
                StateMovement();
                break;

            case States.Stunned:
                stime += Time.deltaTime;
                if (stime >= stuntime)
                {
                    sm = States.Standby;
                    stime = 0;
                }
                break;

            case States.Smoked:

                break;
        }
    }

    public void StateMovement()
    {
        
        switch (sm)
        {
            case States.Standby:
                if (agent.isOnNavMesh)
                {
                    time += Time.deltaTime;
                    if(time >= wanderTime)
                    {
                        destination = RandomNavSphere(freeRoamAA.transform.position, 25, -1);
                        time = 0;
                    }

                    agent.isStopped = false;
                    agent.enabled = true;
                    agent.speed = speed;

                    agent.destination = destination;
                }
                break;
            case States.Chasing:
                if (agent.isOnNavMesh && GetComponentInChildren<Scr_TankTurr>().target)
                {
                    time += Time.deltaTime;
                    if (time >= wanderTime)
                    {
                        destination = RandomNavSphere(GetComponentInChildren<Scr_TankTurr>().target.transform.position, 25, -1);
                        time = 0;
                    }
                    
                    agent.isStopped = false;
                    agent.enabled = true;
                    agent.speed = speed;

                    agent.destination = destination;
                }
                break;
            case States.Stunned:
                
                break;
            case States.Smoked:
                break;
        } 
    }

    public void CreateFRAA()
    {
        freeRoamAA = new GameObject();
        freeRoamAA.transform.position = transform.position;
        freeRoamAA.transform.name = "FRActionArea";
        freeRoamAA.layer = 12;
        destination = RandomNavSphere(freeRoamAA.transform.position, 25, -1);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void Death()
    {
        if( GetComponentInChildren<Scr_Target>().hitPoints <= 0)
        {
            GetComponentInChildren<Scr_Target>().InstantiateExplosion();
            Destroy(gameObject);
        }
    }
}
