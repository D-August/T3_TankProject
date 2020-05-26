using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Target_Re : MonoBehaviour
{
    public GameObject prfb;

    public GameObject[] targets = new GameObject[10];
    public Vector3[] vectors = new Vector3[10];
    public float[] timer;

    public float timelimit = 5;

    void Start()
    {
        for (int i = 0; i < 10; i++) {
            timer[i] = 0;
            vectors[i] = targets[i].transform.position;
        }
    }
    void Update()
    {
        for(int i = 0; i < 10; i++)
        {
            if(targets[i] == null)
            {
                timer[i] += Time.deltaTime;
            }

            if(timer[i] >= timelimit)
            {
                Instantiate(prfb, vectors[i], transform.rotation);
                timer[i] = 0;
            }
        }
    }
}
