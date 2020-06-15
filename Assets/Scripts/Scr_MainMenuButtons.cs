using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MainMenuButtons : MonoBehaviour
{
    public enum States
    {
        Começar_Jogo,
        Créditos,
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

    public void Onclick()
    {
        switch (state)
        {
            case States.Começar_Jogo:
                GameObject.Find("Menu Transition").GetComponent<Scr_MenuTransition>().SceneChange("Tutorial");
                break;
            case States.Créditos:

                break;
            case States.Sair:
                Application.Quit();
                break;
        }
    }
}
