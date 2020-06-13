using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PauseMenu : MonoBehaviour
{
    // Singleton
    public static Scr_PauseMenu pm = null;

    public bool isPaused = false;
    public bool onDialogue = false;
    public GameObject pauseover;

    void Awake()
    {
        if (pm == null) pm = this;
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        PauseControl();
    }

    public void PauseControl()
    {
        if (isPaused || onDialogue)
        {
            Time.timeScale = 0;
            GameObject.Find("Tank (Player)").GetComponent<Scr_Controls_PROT>().PauseAudio();
        }
        else Time.timeScale = 1f;

        pauseover.SetActive(isPaused);
    }

    public void Pause()
    {
        isPaused = !isPaused;
    }

    public void SEDialogue(bool state)
    {
        onDialogue = state;
    }
}
