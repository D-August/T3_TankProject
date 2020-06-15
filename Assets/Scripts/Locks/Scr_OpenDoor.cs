using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OpenDoor : MonoBehaviour
{
    public GameObject door;

    // Update is called once per frame
    void Update()
    {
        door.SetActive(GetComponent<Scr_Lock>().locked);
    }
}
