using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PickItem : MonoBehaviour
{
    [Tooltip("item or ammo")]
    public string type;
    [Tooltip("for item: repair | for ammo: common, shield, smoke or emp")]
    public string oname;
    public int amount;

    public float rotatingSpeed = 25f;

    void Start()
    {
        
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, rotatingSpeed * Time.deltaTime,0));
    }
}
