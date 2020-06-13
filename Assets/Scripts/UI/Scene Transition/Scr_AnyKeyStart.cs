using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AnyKeyStart : MonoBehaviour
{
    public string scenename;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.anyKey)
        {
            GameObject.Find("Menu Transition").GetComponent<Scr_MenuTransition>().SceneChange(scenename);
        }
    }
}
