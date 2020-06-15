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

    public List<GameObject> l_go = new List<GameObject>();

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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (isPaused)
            {
                foreach(GameObject go in l_go)
                {
                    go.SetActive(false);
                }
            }

        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (!isPaused)
            {
                foreach (GameObject go in l_go)
                {
                    go.SetActive(true);
                }
            }
        }

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
