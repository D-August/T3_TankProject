using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_IsGrounded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGround()
    {
        WheelHit hit;
        if (this.GetComponent<WheelCollider>().GetGroundHit(out hit) && hit.collider.gameObject.transform.tag == "Terrain")
        {
            return true;
        }
        else return false;
    }
}
