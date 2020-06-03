using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_PlayerLS : MonoBehaviour
{
    public List<string> validscenes = new List<string>();
    public string lastscene;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ChangeScene("CenaDev");
    }
    void Update()
    {
        if (!validscenes.Contains(SceneManager.GetActiveScene().name)) Destroy(gameObject);
    }

    public void ChangeScene(string scene_name)
    {
        if (SceneManager.GetActiveScene().name != scene_name)
        {
            lastscene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
        }
    }
}
