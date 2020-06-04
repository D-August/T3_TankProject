﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_SceneLoad : MonoBehaviour
{
    [Header("Scene Change")]
    public GameObject pref_tank;
    public GameObject go_player;
    public List<GameObject> l_sp = new List<GameObject>();

    public enum states
    {
        Fadeout,
        Fadein
    }

    [Header("Fade to Black")]
    public Image i_ftb;
    public float c_alpha = 1;
    public float fadespeed= .5f;
    public states s_animte = states.Fadeout;
    private string scenename;

    // Start is called before the first frame update
    void Awake()
    {
        c_alpha = i_ftb.color.a;

        if (GameObject.Find("Tank (Player)")) go_player = GameObject.Find("Tank (Player)");
        else if(pref_tank)
        {
            go_player = Instantiate(pref_tank);
            go_player.transform.name = "Tank (Player)";
            go_player.transform.SetParent(null);
        }

        if(go_player)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "CenaDev":
                    if(go_player.GetComponent<Scr_PlayerLS>().lastscene == "Vila")
                    {
                        go_player.transform.position = l_sp[0].transform.position;
                        go_player.transform.rotation = l_sp[0].transform.rotation;
                    }
                    break;
            }
        }
    }
    void Update()
    {
        switch (s_animte)
        {
            case states.Fadeout:
                if (c_alpha > 0)
                {
                    c_alpha -= fadespeed * Time.deltaTime;
                    i_ftb.color = new Color(i_ftb.color.r, i_ftb.color.g, i_ftb.color.b, c_alpha);

                } 

                break;
            case states.Fadein:
                if (c_alpha < 1)
                {
                    c_alpha += fadespeed * Time.deltaTime;
                    i_ftb.color = new Color(i_ftb.color.r, i_ftb.color.g, i_ftb.color.b, c_alpha);
                }
                else if(c_alpha >= 1)
                {
                    SceneManager.LoadScene(scenename, LoadSceneMode.Single);
                }
                break;
        }
    }

    public void ChangeScene(string scenename)
    {
        if(scenename != SceneManager.GetActiveScene().name)
        {
            if (s_animte == states.Fadeout)
            {
                s_animte = states.Fadein;
            }

            if (GameObject.Find("Tank (Player)"))
            {
                GameObject.Find("Tank (Player)").GetComponent<Scr_PlayerLS>().lastscene = SceneManager.GetActiveScene().name;
            }

            this.scenename = scenename;
        }

    }
}
