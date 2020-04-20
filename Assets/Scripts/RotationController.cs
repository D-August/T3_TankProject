using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    Vector3 PositionOld, PositionNew;
    // Start is called before the first frame update
    void Start()
    {
        PositionNew = transform.position;
        UpdadeRotation();
    }

    void UpdadeRotation()
    {
        PositionOld = PositionNew;
        PositionNew = transform.position;
        Vector3 VecDeslocation = new Vector3(PositionNew.x - PositionOld.x, PositionNew.y - PositionOld.y, PositionNew.z - PositionOld.z);
        gameObject.transform.forward = VecDeslocation.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        UpdadeRotation();
    }
}
