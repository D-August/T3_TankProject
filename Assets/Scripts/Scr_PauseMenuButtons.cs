using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PauseMenuButtons : MonoBehaviour
{
    public enum States
    {
        MuteUnmute,
        Sair
    }

    public States state;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        switch (state)
        {
            case States.MuteUnmute:
                Scr_AudioCon.ac.MuteUnmute();
                break;
            case States.Sair:
                Scr_PauseMenu.pm.Pause();
                GameObject.Find("SceneTransition").GetComponent<Scr_SceneLoad>().ChangeScene("Sc_MainMenu");
                break;
        }
    }
}
