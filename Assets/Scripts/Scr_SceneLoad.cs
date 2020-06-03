using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_SceneLoad : MonoBehaviour
{
    public GameObject pref_tank;
    public GameObject go_player;
    public List<GameObject> l_sp = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
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
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
