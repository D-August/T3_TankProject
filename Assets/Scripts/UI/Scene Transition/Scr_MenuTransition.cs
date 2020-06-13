using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scr_MenuTransition : MonoBehaviour
{
    public enum states
    {
        Fadeout,
        Fadein
    }

    [Header("Fade to Black")]
    public Image i_ftb;
    public float c_alpha = 1;
    public float fadespeed = .5f;
    public states s_animte = states.Fadeout;
    private string scenename;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
                else if (c_alpha >= 1)
                {
                    SceneManager.LoadScene(scenename, LoadSceneMode.Single);
                }
                break;
        }
    }

    public void SceneChange(string scenename)
    {
        if (scenename != "" && c_alpha <= 0 && s_animte == states.Fadeout)
        {
            this.scenename = scenename;
            s_animte = states.Fadein;
        }
    }
}
