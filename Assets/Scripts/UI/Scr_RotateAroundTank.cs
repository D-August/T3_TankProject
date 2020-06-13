using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RotateAroundTank : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public GameObject cam;

    void FixedUpdate()
    {
        Vector3 targetDirection = target.transform.position - cam.transform.position;
        float singleStep = speed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(cam.transform.forward, targetDirection, 40, speed * Time.deltaTime);
        cam.transform.rotation = Quaternion.LookRotation(newDirection);

        transform.Rotate(0, singleStep, 0);
    }
}
