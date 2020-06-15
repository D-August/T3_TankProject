using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_ChangeBosque : MonoBehaviour
{
    public static Scr_ChangeBosque cb = null;

    public bool bosque2 = false;

    public Scr_ColoredKey ck;
    public MeshCollider parede;
    public TextMeshPro tmp;
    public GameObject scenechanger;

    void Awake()
    {
        if (cb == null) cb = this;
    }
    void Update ()
    {
        GameObject temp = GameObject.Find("Tank (Player)");
        if (temp.GetComponent<Scr_Inventory>().GetKey(ck)) bosque2 = true; else bosque2 = false;

        if (!bosque2)
        {
            parede.enabled = true;
            tmp.text = "Path Blocked";
        }
        else
        {
            parede.enabled = false;
            tmp.text = "to Valley";
            scenechanger.GetComponent<Scr_SpawnPointTransition>().scenename = "Bosque 2";
        }
    }
}
