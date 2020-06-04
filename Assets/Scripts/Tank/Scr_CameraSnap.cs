using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraSnap : MonoBehaviour
{
    public GameObject snap;
    public GameObject face;

    public float speed = .75f;
    private Vector3 DesiredDestiny;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!snap)
        {
            if(GameObject.Find("MCSnap"))
            {
                snap = GameObject.Find("MCSnap");
            }
        }

        if (!face)
        {
            if (GameObject.Find("Spawner_Shot"))
            {
                face = GameObject.Find("AimSnap");
            }
        }

        if(snap && face)
        {
            DesiredDestiny = snap.transform.position;
            transform.position = Vector3.Lerp(transform.position, DesiredDestiny, speed * Time.deltaTime);
            Vector3 targetDirection = face.transform.position - transform.position;
            float singleStep = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        
    }
}
