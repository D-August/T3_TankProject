using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AnyKeyStart : MonoBehaviour
{
    public string scenename;
    public bool anykey = true;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.anyKey && anykey)
        {
            GameObject.Find("Menu Transition").GetComponent<Scr_MenuTransition>().SceneChange(scenename);
        }
    }

    public void OnClick()
    {
        GameObject.Find("Menu Transition").GetComponent<Scr_MenuTransition>().SceneChange(scenename);
    }
}
