using System.Collections;
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
    void Reset()
    {
        c_alpha = i_ftb.color.a;

        if (GameObject.Find("Tank (Player)")) go_player = GameObject.Find("Tank (Player)");
        else if (pref_tank)
        {
            go_player = Instantiate(pref_tank);
            go_player.transform.name = "Tank (Player)";
            go_player.transform.SetParent(null);
        }

        if (go_player)
        {
            
            LoadPosition();
        }
    }
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
            // OLD
            /*switch (SceneManager.GetActiveScene().name)
            {
                case "CenaDev":
                    if(go_player.GetComponent<Scr_PlayerLS>().lastscene == "Vila")
                    {
                        go_player.transform.position = l_sp[0].transform.position;
                        go_player.transform.rotation = l_sp[0].transform.rotation;
                    }
                    else
                    {
                        go_player.transform.position = l_sp[0].transform.position;
                        go_player.transform.rotation = l_sp[0].transform.rotation;
                    }
                    break;

                case "Vila":
                    if (go_player.GetComponent<Scr_PlayerLS>().lastscene == "CenaDev")
                    {
                        go_player.transform.position = l_sp[0].transform.position;
                        go_player.transform.rotation = l_sp[0].transform.rotation;
                    }
                    else
                    {
                        go_player.transform.position = l_sp[0].transform.position;
                        go_player.transform.rotation = l_sp[0].transform.rotation;
                    }
                    break;
                default:
                    go_player.transform.position = l_sp[0].transform.position;
                    go_player.transform.rotation = l_sp[0].transform.rotation;
                    break;

            }*/

            // NEW
            LoadPosition();
        }
    }
    void Update()
    {
        if (c_alpha <= 0) i_ftb.enabled = false;
        else i_ftb.enabled = true;

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

    public void LoadPosition()
    {
        string s_temp = go_player.GetComponent<Scr_PlayerLS>().lastscene;
        
        go_player.transform.position = l_sp[GetArrayIndex(s_temp)].transform.position;
        go_player.transform.rotation = l_sp[GetArrayIndex(s_temp)].transform.rotation;
        go_player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
    }

    public int GetArrayIndex(string scenename)
    {
        int i = 0;
        if (scenename.Equals("")) return 0;


        foreach(GameObject go in l_sp)
        {
            if (go.GetComponent<Scr_Spawn>().fromscene == scenename)
            {
                Debug.Log(scenename);
                Debug.Log(go.GetComponent<Scr_Spawn>().fromscene);
                return i;
            }
            i++;
        }

        return 99;
    }
}
