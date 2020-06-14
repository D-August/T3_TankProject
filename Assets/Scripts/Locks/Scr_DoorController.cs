using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DoorController : MonoBehaviour
{
    public List<GameObject> l_locks = new List<GameObject>();

    public GameObject door;
    public List<GameObject> left_locks = new List<GameObject>();
    public bool l_unlocked = false;
    public List<GameObject> right_locks = new List<GameObject>();
    public bool r_unlocked = false;

    public enum State
    {
        Close,
        Open
    }

    public State state;

    void Update()
    {
        if(state == State.Close)
        {
            if (CheckKeys()) state = State.Open;
        }

        if(state == State.Open)
        {
            // Left
            foreach(GameObject g in left_locks)
            {
                g.transform.localPosition = Vector3.MoveTowards(g.transform.localPosition, new Vector3(-1, g.transform.localPosition.y, -27), 2 * Time.deltaTime);
                if (g.transform.localPosition == new Vector3(-1f, g.transform.localPosition.y, -27)) l_unlocked = true;
            }

            // Right
            foreach (GameObject g in right_locks)
            {
                g.transform.localPosition = Vector3.MoveTowards(g.transform.localPosition, new Vector3(-14.5f, g.transform.localPosition.y, 9), 2 * Time.deltaTime);
                if (g.transform.localPosition == new Vector3(-14.5f, g.transform.localPosition.y, 9)) r_unlocked = true;
            }

            //Door
            if(r_unlocked && l_unlocked)
            {
                door.transform.localPosition = Vector3.MoveTowards(door.transform.localPosition, new Vector3(-9.8f, -25, -8.75f), 2 * Time.deltaTime);
            }
        }
    }

    // Check if all the locks are oppened
    public bool CheckKeys()
    {
        foreach(GameObject g in l_locks)
        {
            if (g.GetComponent<Scr_Lock>().locked) return false;
        }

        return true;
    }
}
